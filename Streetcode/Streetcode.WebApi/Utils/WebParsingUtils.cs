using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Polly;
using Microsoft.EntityFrameworkCore;
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

    private static readonly string FileToParseUrl = "https://www.ukrposhta.ua/files/shares/out/houses.zip?_ga=2.213909844.272819342.1674050613-1387315609.1673613938&_gl=1*1obnqll*_ga*MTM4NzMxNTYwOS4xNjczNjEzOTM4*_ga_6400KY4HRY*MTY3NDA1MDYxMy4xMC4xLjE2NzQwNTE3ODUuNjAuMC4w";

    private static readonly (string Prefix, string Type)[] StreetTypeMap = new[]
    {
        ("жилий масив ", "парк"),
        ("пров. ",      "провулок"),
        ("бульв. ",     "бульвар"),
        ("просп. ",     "проспект"),
        ("вул. ",       "вулиця"),
        ("алея ",       "алея"),
        ("бульв. ",     "бульвар"),
        ("дорога ",     "дорога"),
        ("дорога ",     "дорога"),
        ("дорога ",     "дорога"),
        ("дорога ",     "дорога"),
        ("шосе ",       "шосе"),
        ("узвіз ",      "узвіз"),
        ("майдан ",     "майдан"),
        ("пл. ",        "площа"),
        ("парк ",       "парк"),
        ("проїзд ",     "проїзд"),
        ("тупик ",      "тупик"),
        ("м-р ",        "мікрорайон"),
        ("селище ",     "селище"),
        ("хутір ",      "хутір"),
        ("кв-л ",       "квартал"),
        ("урочище ",    "урочище"),
        ("набережна ",  "набережна"),
        ("острів ",     "острів"),
        ("спуск ",      "спуск"),
        ("завулок ",    "завулок"),
        ("лінія ",      "лінія"),
        ("шлях ",       "шлях"),
        ("містечко ",   "містечко"),
        ("в’їзд ",      "в’їзд"),
    };

    private static readonly Dictionary<string, string> StreetTypeReverseMap =
    StreetTypeMap
        .GroupBy(x => x.Type)
        .ToDictionary(
            g => g.Key,
            g => g.First().Prefix);

    private readonly IRepositoryWrapper _repository;
    private readonly StreetcodeDbContext _streetcodeContext;

    public WebParsingUtils(StreetcodeDbContext streetcodeContext)
    {
        _repository = new RepositoryWrapper(streetcodeContext);
        _streetcodeContext = streetcodeContext;
    }

    public async Task ParseZipFileFromWebAsync()
    {
        _ = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;
        var zipPath = $"houses.zip";
        
        // Change the path of extractTo to your path, smth like "/Users/admin/Documents/GitHub/StreetCode/Streetcode/Streetcode.DAL";
        var extractTo = $"/root/build/StreetCode/Streetcode/Streetcode.DAL";

        var cancellationToken = new CancellationTokenSource().Token;

        try
        {
            await DownloadAndExtractAsync(FileToParseUrl, zipPath, extractTo, cancellationToken);
            Console.WriteLine("Download and extraction completed successfully.");

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            await ProcessCsvFileAsync(extractTo, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static async Task DownloadAndExtractAsync(
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
        clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
            3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));

        using var client = new HttpClient(clientHandler, false);
        client.Timeout = TimeSpan.FromSeconds(60);

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
    private async Task ProcessCsvFileAsync(string extractTo, bool deleteFile = false)
    {
        // Following line is required for proper csv encoding
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        string csvPath = $"{extractTo}/data.csv";

        var allLinesFromDataCsv = new List<string>();

        var existingToponyms = await _repository.ToponymRepository.GetAllAsync(predicate: null, include: q => q.Include(t => t.Coordinate));

        // 4. Конвертуємо в рядки CSV
        var header = "Область;Адміністративний район(старий);Адміністративний район(новий);Найменування ОТГ(довідково);Населений пункт;Індекс НП;Назва вулиці";
        var lines = new List<string> { header };

        foreach (var t in existingToponyms)
        {
            var addressColumn = RestoreOriginalAddress(t.StreetName, t.StreetType);
            var lat = t.Coordinate?.Latitude.ToString(CultureInfo.InvariantCulture) ?? "";
            var lon = t.Coordinate?.Longtitude.ToString(CultureInfo.InvariantCulture) ?? "";
            lines.Add($"{t.Oblast};{t.AdminRegionOld};{t.AdminRegionNew};{t.Gromada};{t.Community};;{addressColumn};{lat};{lon}");
        }

        // 5. Записуємо у data.csv
        await File.WriteAllLinesAsync(csvPath, lines, Encoding.GetEncoding(1251));
        Console.OutputEncoding = Encoding.GetEncoding(1251);

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
            .Select(x => x.Split(';').ToList()).ToList();
        /*.Take(20) // TODO take it of if you want to start global parse
        .ToList();*/

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
            string cityStringSearchOptimized = communityCol.Substring(communityCol.IndexOf(" ", StringComparison.Ordinal) + 1);

            var (streetName, streetType) = OptimizeStreetname(row[AddressColumn]);
            string addressRow = $"{cityStringSearchOptimized} {streetName} {streetType}";

            var (latitude, longitude) = await FetchCoordsByAddressAsync(addressRow);

            if (latitude is null || longitude is null)
            {
                addressRow = cityStringSearchOptimized;
                (latitude, longitude) = await FetchCoordsByAddressAsync(addressRow);
            }

            var newRow = string.Empty;
            for (int i = 0; i <= AddressColumn; i++)
            {
                newRow += $"{row[i]};";
            }

            newRow += $"{latitude};{longitude}";

            await File.AppendAllTextAsync(csvPath, newRow + "\n", Encoding.GetEncoding(1251));
        }

        if (deleteFile)
        {
            File.Delete(excelPath);
            Console.WriteLine($"Deleted temporary houses.csv file");
        }

        await SaveToponymsToDbAsync(csvPath);

        if (File.Exists(csvPath))
        {
            File.Delete(csvPath);
            Console.WriteLine($"Deleted temporary data.csv file");
        }
    }

    private async Task SaveToponymsToDbAsync(string csvPath)
    {
        var rows = new List<string>(
                await File.ReadAllLinesAsync(csvPath, Encoding.GetEncoding(1251)))
            .Skip(1)
            .Select(x => x.Split(';'));

        // this part of code truncates Toponyms table
        _streetcodeContext.Set<Toponym>().RemoveRange(_streetcodeContext.Set<Toponym>());
        await _streetcodeContext.SaveChangesAsync();

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
    private static async Task<(string?, string?)> FetchCoordsByAddressAsync(string address)
    {
        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
            3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));

        try
        {
            using var client = new HttpClient();

            // Add user-agent and referer headers to request
            client.DefaultRequestHeaders.Add("User-Agent", "HistoryCode");

            // Send GET request to Nominatim API and retrieve JSON data
            var encodedAddress = Uri.EscapeDataString(address);
            var jsonData = await retryPolicy.WrapAsync(circuitBreakerPolicy).ExecuteAsync(async () =>
                await client.GetByteArrayAsync($"https://nominatim.openstreetmap.org/search?q={encodedAddress}, Україна&format=json&limit=1&addressdetails=1"));

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
    rows.Select(x =>
        {
            var cols = x.Split(';');
            var filtered = cols.Take(beforeColumn).Where((_, idx) => idx != 5).ToList();
            filtered.Insert(5, string.Empty);
            return string.Join(";", filtered);
        })
        .Distinct()
        .ToList();

    // Following method returns name of the street optimized in such kind of way that will allow OSM Nominatim find its coordinates
    private static (string streetName, string streetType) OptimizeStreetname(string streetname)
    {
        foreach (var (prefix, type) in StreetTypeMap)
        {
            if (streetname.StartsWith(prefix, StringComparison.Ordinal))
            {
                var name = streetname[prefix.Length..].Trim();
                return (name, type);
            }
        }

        return (string.Empty, string.Empty);
    }

    /// <summary>
    /// Відновлює оригінальний запис AddressColumn = prefix + streetName.
    /// </summary>
    private static string RestoreOriginalAddress(string streetName, string? streetType)
    {
        if (string.IsNullOrWhiteSpace(streetName) || string.IsNullOrWhiteSpace(streetType))
        {
            return string.Empty;
        }

        if (StreetTypeReverseMap.TryGetValue(streetType, out var prefix))
        {
            return (prefix + streetName).Trim();
        }

        return streetName.Trim();
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