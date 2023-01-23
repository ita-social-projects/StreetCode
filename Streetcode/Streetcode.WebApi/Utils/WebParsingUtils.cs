using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Streetcode.WebApi.Utils
{
    public class WebParsingUtils
    {
        public static async Task DownloadAndExtractAsync(string fileUrl, string zipPath, string extractTo, CancellationToken cancellationToken, ILogger logger = null)
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

        public static async Task ParseZipFileFromWeb(object? state)
        {
            var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;
            var fileUrl = @"https://www.ukrposhta.ua/files/shares/out/houses.zip?_ga=2.213909844.272819342.1674050613-1387315609.1673613938&_gl=1*1obnqll*_ga*MTM4NzMxNTYwOS4xNjczNjEzOTM4*_ga_6400KY4HRY*MTY3NDA1MDYxMy4xMC4xLjE2NzQwNTE3ODUuNjAuMC4w";
            var zipPath = $"{projRootDirectory}/Streetcode.DAL/houses.zip";
            var extractTo = $"{projRootDirectory}/Streetcode.DAL";
            var existingFilePath = Path.Combine(extractTo, "houses.csv");
            if (File.Exists(existingFilePath))
            {
                File.Delete(existingFilePath);
            }

            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            using (var client = new HttpClient(clientHandler, false) { DefaultRequestHeaders = { } })
            {
                using var response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
                response.Content.Headers.ContentType.CharSet = Encoding.GetEncoding(1251).WebName;

                await using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    await using var streamToWriteTo = File.Open(zipPath, FileMode.Create);
                    await streamToReadFrom.CopyToAsync(streamToWriteTo);
                }

                using (var archive = ZipFile.OpenRead(zipPath))
                {
                    Console.WriteLine("Archive received");
                    archive.ExtractToDirectory(extractTo, overwriteFiles: true);
                }

                File.Delete(zipPath);
            }

            await ProcessCSVfile(extractTo, false, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

        public static async Task ParseZipFileFromWeb1(string mode)
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

                if (mode == "full_parse")
                {
                    await ProcessCSVfile(extractTo, false, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
                }
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
        public static async Task ProcessCSVfile(string extractTo, bool deleteFile = false, params int[] columnsToExtract)
        {
                List<AddressEntity> addresses = new List<AddressEntity>();
                string csvPath = $"{extractTo}/data.csv";
                int rowToStartReadingHousesCSV = 0;

                if (File.Exists(csvPath))
                {
                    string[] allLinesFromDataCSV = File.ReadAllLines(csvPath, Encoding.GetEncoding(1251));
                    string lastTextRowFromDataCsv = allLinesFromDataCSV.Last();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.OutputEncoding = Encoding.GetEncoding(1251);
                    Console.WriteLine("CURRENT LAST ROW IS" + lastTextRowFromDataCsv);
                    rowToStartReadingHousesCSV = Array.IndexOf(allLinesFromDataCSV, lastTextRowFromDataCsv);
                    Console.WriteLine(rowToStartReadingHousesCSV);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string excelPath = Directory.GetFiles(extractTo).First(fName => fName.EndsWith("houses.csv"));

                // Following line is required for proper csv encoding
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                string[] rows = File.ReadAllLines(excelPath, Encoding.GetEncoding(1251));

                // Grouping all rows from initial csv in order to get rid of duplicated streets
                var groupedRows = rows
                   .Select(x => x.Split(';'))
                   .GroupBy(x => new { Column0 = x[0], Column1 = x[1], Column2 = x[2], Column3 = x[3], Column4 = x[4], Column6 = x[6] })
                   .ToDictionary(g => g.Key, g => g.ToList());

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Import started. Rows to parse in original csv- " + rows.Length);
                Console.WriteLine("Import started. Rows to parse in grouped csv- " + groupedRows.Count);
                try
                {
                foreach (var row in groupedRows)
                {
                    if (Array.IndexOf(groupedRows.ToArray(), row) >= rowToStartReadingHousesCSV + 1 || (rowToStartReadingHousesCSV == 0))
                    {
                        var columns = row.Key;
                        AddressEntity addressEntityBase = new AddressEntity() { Oblast = columns.Column0, AdminRegionOld = columns.Column1, AdminRegionNew = columns.Column2, Gromada = columns.Column3, City = columns.Column4, StreetName = columns.Column6 };
                        string newRow = null;

                        // OSM Nominatim does not like e.g "м. Вінниця" - use "Вінниця" instead
                        string cityStringSearchOptimized = addressEntityBase.City.Substring(addressEntityBase.City.IndexOf(" ") + 1);

                        string streetNameStringSearchOptimized = OptimizeStreetname(addressEntityBase.StreetName);

                        string addressrow = cityStringSearchOptimized + " " + streetNameStringSearchOptimized;

                        var res = await CallAPICoords(addressrow);
                        if (res.Item1 == "" && res.Item2 == "")
                        {
                            addressrow = cityStringSearchOptimized;
                            res = await CallAPICoords(addressrow);
                        }

                        Console.WriteLine("\n" + addressrow);
                        Console.WriteLine("Coordinates[" + res.Item1 + " " + res.Item2 + "]");
                        addressEntityBase.Lat = res.Item1;
                        addressEntityBase.Lon = res.Item2;
                        newRow = addressEntityBase.Oblast + ";" + addressEntityBase.AdminRegionOld + ";" + addressEntityBase.AdminRegionNew + ";" + addressEntityBase.Gromada + ";" + addressEntityBase.City + ";" + addressEntityBase.PostIndex + ";" + addressEntityBase.StreetName + ";" + res.Item1 + ";" + res.Item2;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.OutputEncoding = Encoding.GetEncoding(1251);

                        // Console.WriteLine(newRow);
                        Console.ForegroundColor = ConsoleColor.White;
                        await File.AppendAllTextAsync(csvPath, newRow + "\n", Encoding.GetEncoding(1251));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Import finished");
                Console.ForegroundColor = ConsoleColor.White;

                if (deleteFile)
                {
                    File.Delete(excelPath);
                }
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
        public static async Task<Tuple<string, string>> CallAPICoords(string address)
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

        public static async Task<bool> SaveToponymToDb(AddressEntity addressEntity)
        {
        }

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
