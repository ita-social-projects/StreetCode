using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

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
            if (string.IsNullOrEmpty(fileUrl) || !Uri.IsWellFormedUriString(fileUrl, UriKind.Absolute))
            {
                throw new ArgumentException("FileUrl cannot be null, empty or invalid", nameof(fileUrl));
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

            using var client = new HttpClient(clientHandler, false) { DefaultRequestHeaders = { }, Timeout = TimeSpan.FromSeconds(60) };
            
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

                using var archive = ZipFile.OpenRead(zipPath);
                
                archive.ExtractToDirectory(extractTo, overwriteFiles: true);
                if (logger != null)
                {
                    logger.LogInformation("Archive received and extracted to {extractTo}", extractTo);
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

        public async Task ParseZipFileFromWebAsync()
        {
            var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;
            string fileUrl = "https://www.ukrposhta.ua/files/shares/out/houses.zip?_ga=2.213909844.272819342.1674050613-1387315609.1673613938&_gl=1*1obnqll*_ga*MTM4NzMxNTYwOS4xNjczNjEzOTM4*_ga_6400KY4HRY*MTY3NDA1MDYxMy4xMC4xLjE2NzQwNTE3ODUuNjAuMC4w";
            var zipPath = $"{projRootDirectory}/Streetcode.DAL/houses.zip";
            var extractTo = $"{projRootDirectory}/Streetcode.DAL";

            var cancellationToken = new CancellationTokenSource().Token;

            int currentTry = 0, numberOfRetries = 3;

            while (currentTry < numberOfRetries)
            {
                try
                {
                    await DownloadAndExtractAsync(fileUrl, zipPath, extractTo, cancellationToken);
                    Console.WriteLine("Download and extraction completed successfully.");
                    if (File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                    }

                    await ProcessCsvFileAsync(extractTo);
                    return;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(10000);
                    currentTry++;
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        /*
        ProcessCsvFileAsync is an async method that reads data from a CSV file and processes it.

        It takes in two parameters:

        extractTo (string): the path to the directory where the CSV file is located
        deleteFile (bool): a flag that indicates whether or not to delete the original CSV file after processing (default is false)
        The method first sets the encoding for the CSV file to be read properly. It then reads the contents of the file at the specified path and stores it in a list.

        It groups the rows from the initial CSV in order to eliminate duplicate streets. It also checks for any rows that have already been processed and removes them from the list of rows to be processed.

        The method then uses the remaining rows to parse the coordinates of the streets and writes them to the CSV file.

        If the deleteFile flag is set to true, the original CSV file will be deleted. The method also saves the toponyms to a database.
        */
        public async Task ProcessCsvFileAsync(string extractTo, bool deleteFile = false)
        {
            // Following line is required for proper csv encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string csvPath = $"{extractTo}/data.csv";

            var allLinesFromDataCSV = new List<string>();

            if (File.Exists(csvPath))
            {
                allLinesFromDataCSV = new List<string>(await File.ReadAllLinesAsync(csvPath, Encoding.GetEncoding(1251)));
                Console.OutputEncoding = Encoding.GetEncoding(1251);
            }

            string excelPath = Directory.GetFiles(extractTo).First(fName => fName.EndsWith("houses.csv"));

            var rows = new List<string>(await File.ReadAllLinesAsync(excelPath, Encoding.GetEncoding(1251)));

            // Grouping all rows from initial csv in order to get rid of duplicated streets

            var forParsingRows = rows
                .Select(x => string.Join(";", x.Split(';').Take(7))).Distinct().ToList();

            var alreadyParsedRows = allLinesFromDataCSV
                .Select(x => string.Join(";", x.Split(';').Take(7))).Distinct().ToList();

            var alreadyParsedRowsToWrite = allLinesFromDataCSV.Distinct();

            var remainsToParse = forParsingRows.Except(alreadyParsedRows)
                .Select(x => x.Split(';').ToList())

                // TODO take it of if you want to start global parse
                .Take(10);

            var toBeDeleted = alreadyParsedRows.Except(forParsingRows);

            Console.WriteLine("Remains to parse: " + remainsToParse.Count());
            Console.WriteLine("To be deleted: " + toBeDeleted.Count());

            // deletes out of date data in data.csv
            foreach (var row in toBeDeleted)
            {
                alreadyParsedRowsToWrite = alreadyParsedRowsToWrite.Where(x => !x.Contains(row)).ToList();
            }

            await File.WriteAllLinesAsync(csvPath, alreadyParsedRowsToWrite, Encoding.GetEncoding(1251));

            // parses coordinates and writes into data.csv
            foreach (var col in remainsToParse)
            {
                string cityStringSearchOptimized = col[4].Substring(col[4].IndexOf(" ") + 1);

                var optimizedSearchString = OptimizeStreetname(col[6]);

                string addressrow = cityStringSearchOptimized + " " + optimizedSearchString.Item1 + " " + optimizedSearchString.Item2;

                (string, string) res = await FetchCoordsByAddressAsync(addressrow);

                if (res.Item1 == string.Empty && res.Item2 == string.Empty)
                {
                    addressrow = cityStringSearchOptimized;
                    res = await FetchCoordsByAddressAsync(addressrow);
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

            await SaveToponymsToDbAsync(csvPath);
        }

        public async Task SaveToponymsToDbAsync(string csvPath)
        {
            var rows = new List<string>(await File.ReadAllLinesAsync(csvPath, Encoding.GetEncoding(1251)))
                .Skip(1).Select(x => x.Split(';'));

            foreach (var col in rows)
            {
                try
                {
                    var name_type = OptimizeStreetname(col[6]);

                    await _repository.ToponymRepository.CreateAsync(new Toponym()
                    {
                        Oblast = col[0],
                        AdminRegionOld = col[1],
                        AdminRegionNew = col[2],
                        Gromada = col[3],
                        Community = col[4],
                        StreetName = name_type.Item1,
                        StreetType = name_type.Item2,
                        Coordinate = new ToponymCoordinate()
                        {
                            Latitude = decimal.Parse(col[7], CultureInfo.InvariantCulture),
                            Longtitude = decimal.Parse(col[8], CultureInfo.InvariantCulture)
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Success: " + (await _repository.SaveChangesAsync() > 0));
        }

        /// <summary>
        /// Fetches the coordinates of an address using the OpenStreetMap Nominatim API.
        /// </summary>
        /// <param name="address">The address to fetch coordinates for.</param>
        /// <returns>A tuple containing the latitude and longitude of the address.</returns>
        public static async Task<(string, string)> FetchCoordsByAddressAsync(string address)
        {
            try
            {
                using var client = new HttpClient();

                // Add user-agent and referer headers to request
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.DefaultRequestHeaders.Add("Referer", "http://www.microsoft.com");

                // Send GET request to Nominatim API and retrieve JSON data
                var jsonData = await client.GetStringAsync("https://nominatim.openstreetmap.org/search?q=" + address + "&format=json&limit=1&addressdetails=1");

                // Parse JSON data to coordinates tuple
                var coords = ParseJsonToCoordinateTuple(jsonData);

                return coords;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return (null, null);
        }

        // Following method returns name of the street optimized in such kind of way that will allow OSM Nominatim find its coordinates
        private static (string?, string?) OptimizeStreetname(string streetname)
        {
            if (streetname.IndexOf("пров. ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "провулок");
            }
            else if (streetname.IndexOf("проїзд ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "проїзд");
            }
            else if (streetname.IndexOf("вул. ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "вулиця");
            }
            else if (streetname.IndexOf("просп. ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "проспект");
            }
            else if (streetname.IndexOf("тупик ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "тупик");
            }
            else if (streetname.IndexOf("пл. ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "площа");
            }
            else if (streetname.IndexOf("майдан ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "майдан");
            }
            else if (streetname.IndexOf("узвіз ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "узвіз");
            }
            else if (streetname.IndexOf("дорога ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "дорога");
            }
            else if (streetname.IndexOf("парк ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "парк");
            }
            else if (streetname.IndexOf("жилий масив ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ", streetname.IndexOf(" ") + 1) + 1), "парк");
            }
            else if (streetname.IndexOf("м-р ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "мікрорайон");
            }
            else if (streetname.IndexOf("алея ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "алея");
            }
            else if (streetname.IndexOf("хутір ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "хутір");
            }
            else if (streetname.IndexOf("кв-л ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "квартал");
            }
            else if (streetname.IndexOf("урочище ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "урочище");
            }
            else if (streetname.IndexOf("набережна ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "набережна");
            }
            else if (streetname.IndexOf("селище ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "селище");
            }
            else if (streetname.IndexOf("лінія ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "лінія");
            }
            else if (streetname.IndexOf("шлях ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "шлях");
            }
            else if (streetname.IndexOf("спуск ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "спуск");
            }
            else if (streetname.IndexOf("завулок ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "завулок");
            }
            else if (streetname.IndexOf("острів ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "острів");
            }
            else if (streetname.IndexOf("бульв. ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "більвар");
            }
            else if (streetname.IndexOf("шосе ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "шосе");
            }
            else if (streetname.IndexOf("містечко ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "містечко");
            }
            else if (streetname.IndexOf("в’їзд ", StringComparison.Ordinal) != -1)
            {
                return (streetname.Substring(streetname.IndexOf(" ") + 1), "в’їзд");
            }

            return (null, null);
        }

        // Following method parses JSON from OSM Nominatim API and returns lat/lon tuple
        private static (string, string) ParseJsonToCoordinateTuple(string json)
        {
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            if (data.Count == 0)
            {
                return (string.Empty, string.Empty);
            }
            else
            {
                return (data[0]["lat"].ToString(), data[0]["lon"].ToString());
            }
        }
    }
}
