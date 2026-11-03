using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesSummary = CreateSalesSummary(salesFiles);

File.WriteAllText(
    Path.Combine(salesTotalDir, "totals.txt"),
    salesSummary
);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(
        folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);

        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

string CreateSalesSummary(IEnumerable<string> salesFiles)
{
    double grandTotal = 0;
    List<string> fileTotals = new List<string>();

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);

        SalesData? data =
            JsonConvert.DeserializeObject<SalesData?>(salesJson);

        double total = data?.Total ?? 0;

        grandTotal += total;

        string fileName = Path.GetRelativePath(
            storesDirectory, file);

        fileTotals.Add($"{fileName}: {total:C2}");
    }

    string summary = $"Sales Summary{Environment.NewLine}";
    summary += $"Grand Total: {grandTotal:C2}{Environment.NewLine}";
    summary += Environment.NewLine;
    summary += "Individual File Totals:" + Environment.NewLine;
    summary += string.Join(Environment.NewLine, fileTotals);

    return summary;
}
record SalesData(double Total);