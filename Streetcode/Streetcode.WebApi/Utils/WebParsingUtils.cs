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

namespace Streetcode.WebApi.Utils
{
    public class WebParsingUtils
    {
        private readonly IRepositoryWrapper _repository;

        public WebParsingUtils(IRepositoryWrapper repository)
        {
            _repository = repository;
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
                .Select(x => string.Join(";", x.Split(';').Take(7).ToList())).Distinct().ToList();

            var alreadyParsedRows = allLinesFromDataCSV
                .Select(x => string.Join(";", x.Split(';').Take(7).ToList())).Distinct().ToList();

            var alreadyParsedRowsToWrite = allLinesFromDataCSV
                .Select(x => string.Join(";", x.Split(';').ToList())).Distinct().ToList();

            var remainsToParse = forParsindRows.Except(alreadyParsedRows).ToList();
            var toBeDeleted = alreadyParsedRows.Except(forParsindRows).ToList();

            Console.WriteLine("Remains to parse: " + remainsToParse.Count());
            Console.WriteLine("To be deleted: " + toBeDeleted.Count());

            foreach (var row in toBeDeleted)
            {
                alreadyParsedRowsToWrite = alreadyParsedRowsToWrite.Where(x => !x.Contains(row)).ToList();
            }

            File.WriteAllLines(csvPath, alreadyParsedRowsToWrite, Encoding.GetEncoding(1251));

            var remainsToParseEntities = remainsToParse.Select(x =>
            {
                var columns = x.Split(';').ToList();
                return new AddressEntity()
                {
                    Oblast = columns[0],
                    AdminRegionOld = columns[1],
                    AdminRegionNew = columns[2],
                    Gromada = columns[3],
                    City = columns[4],
                    PostIndex = columns[5],
                    StreetName = columns[6]
                };
            })
            .ToList();

            // foreach (var row in remainsToParseEntities)
            // {
            //    Console.WriteLine(row.Oblast + ' ' + row.AdminRegionOld + ' ' + row.AdminRegionNew + ' ' + row.Gromada + ' ' + row.City + ' ' + row.StreetName);
            // }
            for (int i = 0; i < remainsToParseEntities.Count(); i++)
            {
                string cityStringSearchOptimized = remainsToParseEntities[i].City.Substring(remainsToParseEntities[i].City.IndexOf(" ") + 1);

                string streetNameStringSearchOptimized = OptimizeStreetname(remainsToParseEntities[i].StreetName);

                string addressrow = cityStringSearchOptimized + " " + streetNameStringSearchOptimized;

                var res = await CallAPICoords(addressrow);
                if (res.Item1 == "" && res.Item2 == "")
                {
                    addressrow = cityStringSearchOptimized;
                    res = await CallAPICoords(addressrow);
                }

                Console.WriteLine("\n" + addressrow);
                Console.WriteLine("Coordinates[" + res.Item1 + " " + res.Item2 + "]");

                remainsToParseEntities[i].Lat = res.Item1;
                remainsToParseEntities[i].Lon = res.Item2;

                string newRow = remainsToParseEntities[i].Oblast + ";" + remainsToParseEntities[i].AdminRegionOld + ";" +
                    remainsToParseEntities[i].AdminRegionNew + ";" + remainsToParseEntities[i].Gromada + ";" +
                    remainsToParseEntities[i].City + ";" + remainsToParseEntities[i].PostIndex + ";"
                    + remainsToParseEntities[i].StreetName + ";" + res.Item1 + ";" + res.Item2;

                Console.WriteLine(newRow);
                await File.AppendAllTextAsync(csvPath, newRow + "\n", Encoding.GetEncoding(1251));
            }

            if (deleteFile)
            {
                File.Delete(excelPath);
            }

            // await SaveToponymsToDb(csvPath);
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

        // public async Task SaveToponymsToDb(string csvPath)
        // {
        //    List<string> rows = new List<string>(File.ReadAllLines(csvPath, Encoding.GetEncoding(1251)));
        //    foreach (var ad in adresses)
        //    {
        //        try
        //        {
        //            Toponym toponym = new Toponym()
        //            {
        //                Oblast = ad.Oblast,
        //                AdminRegionOld = ad.AdminRegionOld,
        //                AdminRegionNew = ad.AdminRegionNew,
        //                Gromada = ad.Gromada,
        //                Community = ad.City,
        //                StreetName = ad.StreetName,
        //                Coordinates = new List<ToponymCoordinate>(),
        //                Streetcodes = new List<StreetcodeContent>(),
        //            };
        //            await _repository.ToponymRepository.CreateAsync(toponym);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }

        // Console.WriteLine(await _repository.SaveChangesAsync() > 0);
        //  }

        public class AddressEntity
        {
            public string Oblast { get; set; }
            public string AdminRegionOld { get; set; }
            public string AdminRegionNew { get; set; }
            public string Gromada { get; set; }
            public string City { get; set; }
            public string PostIndex { get; set; }
            public string StreetName { get; set; }
            public string Lat { get; set; }
            public string Lon { get; set; }
        }
    }
}
