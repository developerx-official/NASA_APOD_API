![issues](https://img.shields.io/github/issues/developerx-official/NASA_APOD_API) ![forks](https://img.shields.io/github/forks/developerx-official/NASA_APOD_API) ![stars](https://img.shields.io/github/stars/developerx-official/NASA_APOD_API) ![license](https://img.shields.io/github/license/developerx-official/NASA_APOD_API) ![codeql-analysis](https://github.com/developerx-official/NASA_APOD_API/actions/workflows/codeql-analysis.yml/badge.svg)

# NASA_APOD_API

## How to Install

Using nuget, install `NASA_APOD`.

(`dotnet add package NASA_APOD`)

## How to Use

* Instantiate APOD_CLIENT (such as on app enter)
* Query with yourClient.Query(your, parameters);
* Dispose when no longer being used (such as on app exit)

## Short Example

```csharp
// In practice try to use one client per application as it can stress socket connections
// This is just showing that it can be disposed (such as with a using statement)
// It also uses the demo key, please don't do this to yourself, getting an actual api key is painless on their website
// https://api.nasa.gov/#signUp
using (APOD_Client client = new APOD_Client("DEMO_KEY"))
{
    var result = await client.QueryAsync();
    Console.WriteLine($"TITLE: {result[0].title}\n" +
                      $"URL: {result[0].url}");
}
```

