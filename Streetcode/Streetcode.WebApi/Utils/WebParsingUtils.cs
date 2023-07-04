using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Polly;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.WebApi.Utils;

public class WebParsingUtils
{
    private const byte RegionColumn = 0;
    private const byte AdministrativeRegionOldColumn = 1;
    private const byte AdministrativeRegionNewColumn = 2;
    private const byte CommonalityColumn = 3;
    private const byte CommunityColumn = 4;
    private const byte AddressColumn = 6;
    private const byte LatitudeColumn = 7;
    private const byte LongitudeColumn = 8;

    private static readonly string _fileToParseUrl = "https://www.ukrposhta.ua/files/shares/out/houses.zip?_ga=2.213909844.272819342.1674050613-1387315609.1673613938&_gl=1*1obnqll*_ga*MTM4NzMxNTYwOS4xNjczNjEzOTM4*_ga_6400KY4HRY*MTY3NDA1MDYxMy4xMC4xLjE2NzQwNTE3ODUuNjAuMC4w";

    private readonly IRepositoryWrapper _repository;
    private readonly StreetcodeDbContext _streetcodeContext;

    public WebParsingUtils(StreetcodeDbContext streetcodeContext)
    {
        _repository = new RepositoryWrapper(streetcodeContext);
        _streetcodeContext = streetcodeContext;
    }

    public static async Task DownloadAndExtractAsync(
        string fileUrl,
        string zipPath,
        string extractTo,
        CancellationToken cancellationToken)
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

        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
            3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));

        using var client = new HttpClient(clientHandler, false)
        {
            DefaultRequestHeaders = { },
            Timeout = TimeSpan.FromSeconds(60)
        };

        try
        {
            using var response = await retryPolicy.WrapAsync(circuitBreakerPolicy).ExecuteAsync(async () => await client
                .GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken));

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType!.CharSet = Encoding.GetEncoding(1251).WebName;

            await using (var streamToReadFrom = await response.Content.ReadAsStreamAsync(cancellationToken))
            {
                await using var streamToWriteTo = File.Open(zipPath, FileMode.Create);
                await streamToReadFrom.CopyToAsync(streamToWriteTo, 81920, cancellationToken);
            }

            using var archive = ZipFile.OpenRead(zipPath);
            archive.ExtractToDirectory(extractTo, overwriteFiles: true);
            Console.WriteLine($"Archive received and extracted to {extractTo}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("The operation was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task ParseZipFileFromWebAsync()
    {
        var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;
        var zipPath = $"houses.zip";
        var extractTo = $"/root/build/StreetCode/Streetcode/Streetcode.DAL";

        var cancellationToken = new CancellationTokenSource().Token;

        try
        {
            await DownloadAndExtractAsync(_fileToParseUrl, zipPath, extractTo, cancellationToken);
            Console.WriteLine("Download and extraction completed successfully.");

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            await ProcessCsvFileAsync(extractTo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
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

        var allLinesFromDataCsv = new List<string>();

        if (File.Exists(csvPath))
        {
            allLinesFromDataCsv = new List<string>(await File.ReadAllLinesAsync(csvPath, Encoding.GetEncoding(1251)));
            Console.OutputEncoding = Encoding.GetEncoding(1251);
        }

        string excelPath = Directory.GetFiles(extractTo).First(fName => fName.EndsWith("houses.csv"));

        var rows = new List<string>(await File.ReadAllLinesAsync(excelPath, Encoding.GetEncoding(1251)));

        // Grouping all rows from initial csv in order to get rid of duplicated streets

        var forParsingRows = GetDistinctRows(rows);

        var alreadyParsedRows = GetDistinctRows(allLinesFromDataCsv);

        var alreadyParsedRowsToWrite = allLinesFromDataCsv.Distinct().ToList();

        var remainsToParse = forParsingRows.Except(alreadyParsedRows)
            .Select(x => x.Split(';').ToList()).ToList()
            .Take(20) // TODO take it of if you want to start global parse
            .ToList();

        var toBeDeleted = alreadyParsedRows.Except(forParsingRows).ToList();

        Console.WriteLine("Remains to parse: " + remainsToParse.Count);
        Console.WriteLine("To be deleted: " + toBeDeleted.Count);

        // deletes out of date data in data.csv
        foreach (var row in toBeDeleted)
        {
            alreadyParsedRowsToWrite = alreadyParsedRowsToWrite.Where(x => !x.Contains(row)).ToList();
        }

        await File.WriteAllLinesAsync(csvPath, alreadyParsedRowsToWrite, Encoding.GetEncoding(1251));

        // parses coordinates and writes into data.csv
        foreach (var row in remainsToParse)
        {
            var communityCol = row[CommunityColumn];
            string cityStringSearchOptimized = communityCol.Substring(communityCol.IndexOf(" ") + 1);

            var (streetName, streetType) = OptimizeStreetname(row[AddressColumn]);
            string addressRow = $"{cityStringSearchOptimized} {streetName} {streetType}";

            var (latitude, longitude) = await FetchCoordsByAddressAsync(addressRow);

            if (latitude is null || longitude is null)
            {
                addressRow = cityStringSearchOptimized;
                (latitude, longitude) = await FetchCoordsByAddressAsync(addressRow);
            }

            Console.WriteLine("\n" + addressRow);
            Console.WriteLine($"Coordinates[{latitude}-{longitude}]");

            var newRow = string.Empty;
            for (int i = 0; i <= AddressColumn; i++)
            {
                newRow += $"{row[i]};";
            }

            newRow += $"{latitude};{longitude}";
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
        var rows = new List<string>(
                await File.ReadAllLinesAsync(csvPath, Encoding.GetEncoding(1251)))
            .Skip(1)
            .Select(x => x.Split(';'));

        // this part of code truncates Toponyms table
        _streetcodeContext.Set<Toponym>().RemoveRange(_streetcodeContext.Set<Toponym>());
        _streetcodeContext.SaveChanges();

        foreach (var row in rows)
        {
            try
            {
                var (streetName, streetType) = OptimizeStreetname(row[AddressColumn]);

                await _repository.ToponymRepository.CreateAsync(new Toponym
                {
                    Oblast = row[RegionColumn],
                    AdminRegionOld = row[AdministrativeRegionOldColumn],
                    AdminRegionNew = row[AdministrativeRegionNewColumn],
                    Gromada = row[CommonalityColumn],
                    Community = row[CommunityColumn],
                    StreetName = streetName,
                    StreetType = streetType,
                    Coordinate = new ToponymCoordinate
                    {
                        Latitude = decimal.Parse(row[LatitudeColumn], CultureInfo.InvariantCulture),
                        Longtitude = decimal.Parse(row[LongitudeColumn], CultureInfo.InvariantCulture)
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        var isChangeSuccessful = await _repository.SaveChangesAsync() > 0;
        Console.WriteLine($"Success: {isChangeSuccessful}");
    }

    /// <summary>
    /// Fetches the coordinates of an address using the OpenStreetMap Nominatim API.
    /// </summary>
    /// <param name="address">The address to fetch coordinates for.</param>
    /// <returns>A tuple containing the latitude and longitude of the address.</returns>
    public static async Task<(string?, string?)> FetchCoordsByAddressAsync(string address)
    {
        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
            3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));

        try
        {
            using var client = new HttpClient();

            // Add user-agent and referer headers to request
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.DefaultRequestHeaders.Add("Referer", "http://www.microsoft.com");

            // Send GET request to Nominatim API and retrieve JSON data
            var jsonData = await retryPolicy.WrapAsync(circuitBreakerPolicy).ExecuteAsync(async () =>
                await client.GetByteArrayAsync($"https://nominatim.openstreetmap.org/search?q={address}, Україна&format=json&limit=1&addressdetails=1"));

            return ParseJsonToCoordinateTuple(Encoding.UTF8.GetString(jsonData));
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        return (null, null);
    }

    /// <summary>
    /// Returns a list of distinct rows based on the first <b>N</b> columns of the input list.
    /// </summary>
    /// <param name="rows">The list of rows to filter.</param>
    /// <param name="beforeColumn">How many columns will be taken.</param>
    /// <returns>A list of distinct rows based on the first seven columns of the input list.</returns>
    private static List<string> GetDistinctRows(IEnumerable<string> rows, byte beforeColumn = 7) =>
        rows.Select(x => string.Join(";", x.Split(';').Take(beforeColumn)))
            .Distinct()
            .ToList();

    // Following method returns name of the street optimized in such kind of way that will allow OSM Nominatim find its coordinates
    private static (string, string) OptimizeStreetname(string streetname)
    {
        if (streetname.IndexOf("пров. ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "провулок");
        }

        if (streetname.IndexOf("проїзд ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "проїзд");
        }

        if (streetname.IndexOf("вул. ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "вулиця");
        }

        if (streetname.IndexOf("просп. ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "проспект");
        }

        if (streetname.IndexOf("тупик ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "тупик");
        }

        if (streetname.IndexOf("пл. ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "площа");
        }

        if (streetname.IndexOf("майдан ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "майдан");
        }

        if (streetname.IndexOf("узвіз ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "узвіз");
        }

        if (streetname.IndexOf("дорога ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "дорога");
        }

        if (streetname.IndexOf("парк ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "парк");
        }

        if (streetname.IndexOf("жилий масив ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ", streetname.IndexOf(" ") + 1) + 1), "парк");
        }

        if (streetname.IndexOf("м-р ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "мікрорайон");
        }

        if (streetname.IndexOf("алея ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "алея");
        }

        if (streetname.IndexOf("хутір ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "хутір");
        }

        if (streetname.IndexOf("кв-л ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "квартал");
        }

        if (streetname.IndexOf("урочище ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "урочище");
        }

        if (streetname.IndexOf("набережна ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "набережна");
        }

        if (streetname.IndexOf("селище ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "селище");
        }

        if (streetname.IndexOf("лінія ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "лінія");
        }

        if (streetname.IndexOf("шлях ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "шлях");
        }

        if (streetname.IndexOf("спуск ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "спуск");
        }

        if (streetname.IndexOf("завулок ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "завулок");
        }

        if (streetname.IndexOf("острів ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "острів");
        }

        if (streetname.IndexOf("бульв. ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "бульвар");
        }

        if (streetname.IndexOf("шосе ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "шосе");
        }

        if (streetname.IndexOf("містечко ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "містечко");
        }

        if (streetname.IndexOf("в’їзд ", StringComparison.Ordinal) != -1)
        {
            return (streetname.Substring(streetname.IndexOf(" ") + 1), "в’їзд");
        }

        return (string.Empty, string.Empty);
    }

    // Following method parses JSON from OSM Nominatim API and returns lat/lon tuple
    private static (string?, string?) ParseJsonToCoordinateTuple(string json)
    {
        var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

        if (data is null || data.Count == 0)
        {
            return default;
        }

        return (data[0]["lat"].ToString(), data[0]["lon"].ToString());
    }
}