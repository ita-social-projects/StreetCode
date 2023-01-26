using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;
using Streetcode.DAL.Entities;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Persistence;

namespace Streetcode.WebApi.Utils
{
    public class WebParsingUtils
    {
        private readonly IRepositoryWrapper _repository;

        public WebParsingUtils(StreetcodeDbContext streetcodeContext)
        {
            _repository = new RepositoryWrapper(streetcodeContext);
        }

        public async Task DownloadAndExtractAsync(string fileUrl, string zipPath, string extractTo, CancellationToken cancellationToken, ILogger logger = null)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new ArgumentException("FileUrl cannot be null or empty", nameof(fileUrl));
            }

            if (!Uri.IsWellFormedUriString(fileUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid FileUrl", nameof(fileUrl));
            }

            if (string.IsNullOrEmpty(zipPath))
            {
                throw new ArgumentException("zipPath cannot be null or empty", nameof(zipPath));
            }

            if (string.IsNullOrEmpty(extractTo))
            {
                throw new ArgumentException("extractTo cannot be null or empty", nameof(extractTo));
            }

            var clientHandler = new HttpClientHandler();
            clientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(clientHandler, false) { DefaultRequestHeaders = { }, Timeout = TimeSpan.FromSeconds(60) })
            {
                try
                {
                    using var response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    response.Content.Headers.ContentType.CharSet = Encoding.GetEncoding(1251).WebName;

                    await using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
                    {
                        await using var streamToWriteTo = File.Open(zipPath, FileMode.Create);
                        await streamToReadFrom.CopyToAsync(streamToWriteTo, 81920, cancellationToken);
                    }

                    using (var archive = ZipFile.OpenRead(zipPath))
                    {
                        archive.ExtractToDirectory(extractTo, overwriteFiles: true);
                        if (logger != null)
                        {
                            logger.LogInformation("Archive received and extracted to {extractTo}", extractTo);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    if (logger != null)
                    {
                        logger.LogWarning("The operation was cancelled.");
                    }

                    throw;
                }
                catch (Exception ex)
                {
                    if (logger != null)
                    {
                        logger.LogError(ex, "An error occurred while downloading and extracting the archive");
                    }

                    throw;
                }
            }
        }

        public async Task ParseZipFileFromWeb()
        {
            var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;
            string fileUrl = "https://www.ukrposhta.ua/files/shares/out/houses.zip?_ga=2.213909844.272819342.1674050613-1387315609.1673613938&_gl=1*1obnqll*_ga*MTM4NzMxNTYwOS4xNjczNjEzOTM4*_ga_6400KY4HRY*MTY3NDA1MDYxMy4xMC4xLjE2NzQwNTE3ODUuNjAuMC4w";
            var zipPath = $"{projRootDirectory}/Streetcode.DAL/houses.zip";
            var extractTo = $"{projRootDirectory}/Streetcode.DAL";

            var cancellationToken = new CancellationTokenSource().Token;

            try
            {
                await DownloadAndExtractAsync(fileUrl, zipPath, extractTo, cancellationToken);
                Console.WriteLine("Download and extraction completed successfully.");
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                await ProcessCSVfile(extractTo, false);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("The operation was canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        // This method processes initial csv file and creates data.csv, where all duplicated addresses are removed in order to optimize the script.
        // This script also calls OSM Nominatim API in order to get lat and lon of current address row
        public async Task ProcessCSVfile(string extractTo, bool deleteFile = false)
        {
            // Following line is required for proper csv encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string csvPath = $"{extractTo}/data.csv";

            List<string> allLinesFromDataCSV = new List<string>();

            if (File.Exists(csvPath))
            {
                allLinesFromDataCSV = new List<string>(File.ReadAllLines(csvPath, Encoding.GetEncoding(1251)));
                Console.OutputEncoding = Encoding.GetEncoding(1251);
            }

            string excelPath = Directory.GetFiles(extractTo).First(fName => fName.EndsWith("houses.csv"));

            List<string> rows = new List<string>(File.ReadAllLines(excelPath, Encoding.GetEncoding(1251)));

            // Grouping all rows from initial csv in order to get rid of duplicated streets

            var forParsindRows = rows
                .Select(x => string.Join(";", x.Split(';').Take(7).ToList())).Distinct();

            var alreadyParsedRows = allLinesFromDataCSV
                .Select(x => string.Join(";", x.Split(';').Take(7).ToList())).Distinct();

            var alreadyParsedRowsToWrite = allLinesFromDataCSV.Distinct();

            var remainsToParse = forParsindRows.Except(alreadyParsedRows)
                .Select(x => x.Split(';').ToList())
                .Take(10)
                .ToList();

            var toBeDeleted = alreadyParsedRows.Except(forParsindRows);

            Console.WriteLine("Remains to parse: " + remainsToParse.Count());
            Console.WriteLine("To be deleted: " + toBeDeleted.Count());

            // deletes out of date data in data.csv
            foreach (var row in toBeDeleted)
            {
                alreadyParsedRowsToWrite = alreadyParsedRowsToWrite.Where(x => !x.Contains(row)).ToList();
            }

            File.WriteAllLines(csvPath, alreadyParsedRowsToWrite, Encoding.GetEncoding(1251));

            // parses coordinates and writes into data.csv
            foreach (var col in remainsToParse)
            {
                string cityStringSearchOptimized = col[4].Substring(col[4].IndexOf(" ") + 1);

                string streetNameStringSearchOptimized = OptimizeStreetname(col[6]);

                string addressrow = cityStringSearchOptimized + " " + streetNameStringSearchOptimized;

                var res = await CallAPICoords(addressrow);
                if (res.Item1 == "" && res.Item2 == "")
                {
                    addressrow = cityStringSearchOptimized;
                    res = await CallAPICoords(addressrow);
                }

                Console.WriteLine("\n" + addressrow);
                Console.WriteLine("Coordinates[" + res.Item1 + " " + res.Item2 + "]");

                string newRow = col[0] + ";" + col[1] + ";" +
                    col[2] + ";" + col[3] + ";" +
                    col[4] + ";" + col[5] + ";"
                    + col[6] + ";" + res.Item1 + ";" + res.Item2;

                Console.WriteLine(newRow);
                await File.AppendAllTextAsync(csvPath, newRow + "\n", Encoding.GetEncoding(1251));
            }

            if (deleteFile)
            {
                File.Delete(excelPath);
            }

            await SaveToponymsToDb(csvPath);
        }

        public async Task SaveToponymsToDb(string csvPath)
        {
            var rows = new List<string>(File.ReadAllLines(csvPath, Encoding.GetEncoding(1251)))
                .Select(x => x.Split(';').ToList()).ToList();

            foreach (var col in rows)
            {
                try
                {
                    await _repository.ToponymRepository.CreateAsync(new Toponym()
                    {
                        Oblast = col[0],
                        AdminRegionOld = col[1],
                        AdminRegionNew = col[2],
                        Gromada = col[3],
                        Community = col[4],
                        StreetName = col[6],
                        Streetcodes = new List<StreetcodeContent>(),
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Success: " + (await _repository.SaveChangesAsync() > 0));
        }

        // Following method returns name of the street optimized in such kind of way that will allow OSM Nominatim find its coordinates
        public static string OptimizeStreetname(string streetname)
        {
            string streetNameStringSearchOptimized = streetname;

            // OSM Nominatim does not like e.g "пров. 1-й Тихий", use "1-й Тихий провулок" instead
            if (streetname.IndexOf("пров.") != -1)
            {
                streetNameStringSearchOptimized = streetname.Substring(streetname.IndexOf(" ") + 1) + " провулок";
            }

            // OSM Nominatim does not like e.g "проїзд 3-й Цукровиків", use "3-й Цукровиків проїзд" instead
            else if (streetname.IndexOf("проїзд") != -1)
            {
                streetNameStringSearchOptimized = streetname.Substring(streetname.IndexOf(" ") + 1) + " проїзд";
            }

            // OSM Nominatim does not like e.g "бульв. Івана  Світличного", use "Івана  Світличного бульвар" instead
            else if (streetname.IndexOf("бульв.") != -1)
            {
                streetNameStringSearchOptimized = streetname.Substring(streetname.IndexOf(" ") + 1) + " бульвар";
            }

            // OSM Nominatim does not like e.g "шосе Стратегічне", use "Стратегічне шосе" instead
            else if (streetname.IndexOf("шосе ") != -1)
            {
                streetNameStringSearchOptimized = streetname.Substring(streetname.IndexOf(" ") + 1) + " шосе";
            }

            // OSM Nominatim does not like e.g "містечко Військове", use "Військове містечко" instead
            else if (streetname.IndexOf("містечко ") != -1)
            {
                streetNameStringSearchOptimized = streetname.Substring(streetname.IndexOf(" ") + 1) + " містечко";
            }

            // OSM Nominatim does not like e.g "в’їзд 2-й Криничний", use "2-й Криничний в’їзд" instead
            else if (streetname.IndexOf("в’їзд") != -1)
            {
                streetNameStringSearchOptimized = streetname.Substring(streetname.IndexOf(" ") + 1) + " в’їзд";
            }

            return streetNameStringSearchOptimized;
        }

        // Following method calls OSM Nominatim API for single address and returns lat and lon coordinates
        // For some addresses nothing can be returned
        public static async Task<Tuple<string, string>?> CallAPICoords(string address)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                webClient.Headers.Add("Referer", "http://www.microsoft.com");
                var jsonData = webClient.DownloadData("https://nominatim.openstreetmap.org/search?q=" + address + "&format=json&limit=1&addressdetails=1");
                string jsonStr = Encoding.UTF8.GetString(jsonData);
                Tuple<string, string> coords = ParseJsonToCoordinateTuple(jsonStr);
                return coords;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return null;
        }

        // Following method parses JSON from OSM Nominatim API and returns lat/lon tuple
        public static Tuple<string, string> ParseJsonToCoordinateTuple(string json)
        {
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            if (data.Count == 0)
            {
                return new Tuple<string, string>(string.Empty, string.Empty);
            }
            else
            {
                return new Tuple<string, string>(data[0]["lat"].ToString(), data[0]["lon"].ToString());
            }
        }
    }
}
