using System.IO.Compression;
using System.Text;
using OfficeOpenXml;

namespace Streetcode.WebApi.Utils;

public class WebParsingUtils
{
    public static async void ParseZipFileFromWeb(object? state)
    {
        var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;

        var fileUrl = @"https://www.ukrposhta.ua/files/shares/out/postindex.zip?_ga=2.154835320.1775361449.1673626930-13
                           10026737.1673626930&_gl=1*bx4o4d*_ga*MTMxMDAyNjczNy4xNjczNjI2OTMw*_ga_6400KY4HRY*MTY3MzY4MTA5Ny4zLjA
                           uMTY3MzY4MTA5OC41OS4wLjA.";

        var zipPath = $"{projRootDirectory}/Streetcode.DAL/postindex.zip";
        var extractTo = $"{projRootDirectory}/Streetcode.DAL";

        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

        using var client = new HttpClient(clientHandler);
        using var response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);

        await using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
        {
            await using var streamToWriteTo = File.Open(zipPath, FileMode.Create);
            await streamToReadFrom.CopyToAsync(streamToWriteTo);
        }

        using (var archive = ZipFile.OpenRead(zipPath))
        {
            archive.ExtractToDirectory(extractTo, overwriteFiles: true);
        }

        File.Delete(zipPath);

        await ParseExcelToCsv(extractTo, false, 2, 5, 6);
    }

    public static async Task ParseExcelToCsv(string extractTo, bool deleteFile = false, params int[] columnsToExtract)
    {
        string csvPath = $"{extractTo}/data.csv";
        string excelPath = Directory.GetFiles(extractTo).First(fName => fName.EndsWith(".xlsx"));

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new FileInfo(excelPath));
        var worksheet = package.Workbook.Worksheets[0];

        var rows = new HashSet<string>();
        var stringBuilder = new StringBuilder();
        for (int i = 1; i <= worksheet.Dimension.End.Row; i++)
        {
            var cellIdx = i;
            var newRow = string.Join(",", columnsToExtract.Select(
                c => worksheet.Cells[cellIdx, c].Value?.ToString()));

            if (!rows.Contains(newRow))
            {
                rows.Add(newRow);
                stringBuilder.AppendLine(newRow);
            }
        }

        await File.WriteAllTextAsync(csvPath, stringBuilder.ToString(), Encoding.UTF8);

        if (deleteFile)
        {
            File.Delete(excelPath);
        }
    }
}