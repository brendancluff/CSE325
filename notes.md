# CSE 325 – Week 01 .NET Applications

## 1. ASP.NET Core Web API – Pizza Inventory

I completed the **Create a web API with ASP.NET Core controllers** module and extended the ContosoPizza application by adding a third pizza, Pepperoni, to the initial inventory.

### Existing Pizzas and Additional Pizza

I verified the inventory by sending a GET request:

**Request:**

```http
GET http://localhost:5285/pizza
```

**Response (HTTP 200 OK):**

```json
[
  {
    "id": 1,
    "name": "Classic Italian",
    "isGlutenFree": false
  },
  {
    "id": 2,
    "name": "Veggie",
    "isGlutenFree": true
  },
  {
    "id": 3,
    "name": "Pepperoni",
    "isGlutenFree": false
  }
]
```

The additional pizza is **Pepperoni (ID 3)**. This demonstrates that the API returns the original pizzas along with the new pizza I added.

## 2. API CRUD Operations

I tested the four CRUD operations using PowerShell and the ContosoPizza API running locally on port 5285.

### GET – Retrieve a Pizza

**Request:**

```powershell
Invoke-WebRequest -Uri "http://localhost:5285/pizza/4" -UseBasicParsing | Select-Object StatusCode, Content
```

**Response:**

```text
StatusCode Content
---------- -------
       200 {"id":4,"name":"Hawaiian","isGlutenFree":false}
```

**Status Code:** 200 OK

The GET request successfully retrieved the Hawaiian pizza created during the POST test.

### POST – Create a Pizza

**Request:**

```powershell
Invoke-WebRequest -Uri "http://localhost:5285/pizza" `
    -Method Post `
    -ContentType "application/json" `
    -Body '{"name":"Hawaiian","isGlutenFree":false}' `
    -UseBasicParsing | Select-Object StatusCode, Content
```

**Response:**

```text
StatusCode Content
---------- -------
       201 {"id":4,"name":"Hawaiian","isGlutenFree":false}
```

**Status Code:** 201 Created

The POST request successfully created a new Hawaiian pizza with ID 4.

### PUT – Update a Pizza

**Request:**

```powershell
Invoke-WebRequest -Uri "http://localhost:5285/pizza/3" `
    -Method Put `
    -ContentType "application/json" `
    -Body '{"id":3,"name":"Spicy Pepperoni","isGlutenFree":false}' `
    -UseBasicParsing | Select-Object StatusCode, Content
```

**Response:**

```text
StatusCode Content
---------- -------
       204 {}
```

**Status Code:** 204 No Content

The PUT request successfully updated pizza ID 3 from Pepperoni to Spicy Pepperoni.

I verified the update using GET:

```text
StatusCode Content
---------- -------
       200 {"id":3,"name":"Spicy Pepperoni","isGlutenFree":false}
```

### DELETE – Remove a Pizza

**Request:**

```powershell
Invoke-WebRequest -Uri "http://localhost:5285/pizza/3" `
    -Method Delete `
    -UseBasicParsing | Select-Object StatusCode, Content
```

**Response:**

```text
StatusCode Content
---------- -------
       204 {}
```

**Status Code:** 204 No Content

The DELETE request successfully removed pizza ID 3.

I verified the deletion by sending another GET request for pizza ID 3.

**Verification Response:**

```text
HTTP Status Code: 404
```

**Status Code:** 404 Not Found

This confirmed that the deleted pizza was no longer available.

### CRUD Test Summary

| Operation | Status Code | Result |
|---|---|---|
| GET | 200 OK | Retrieved Hawaiian pizza |
| POST | 201 Created | Created Hawaiian pizza |
| PUT | 204 No Content | Updated Pepperoni to Spicy Pepperoni |
| DELETE | 204 No Content | Deleted Pepperoni pizza |
| GET verification | 404 Not Found | Confirmed pizza was deleted |

## 3. Sales Summary Function

For the **Work with files and directories in a .NET app** module, I implemented a `CreateSalesSummary` function that reads sales data from JSON files, calculates the total sales amount, and generates a formatted summary containing the grand total and individual file totals.

**Working C# Function:**

```csharp
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
```

**Supporting Data Type:**

```csharp
record SalesData(double Total);
```

### Results

The function successfully generated a sales summary file at:

```text
mslearn-dotnet-files/salesTotalDir/totals.txt
```

The resulting summary included:

- A heading identifying the sales summary.
- A calculated grand total of **$2,012.20**.
- Individual sales totals for each JSON file processed.

This demonstrates working with directories, reading JSON files, deserializing data, performing calculations, and writing formatted results to a text file.

## Repository

GitHub: https://github.com/brendancluff/CSE325

The repository contains the completed .NET exercises, ContosoPizza API, and sales summary implementation.