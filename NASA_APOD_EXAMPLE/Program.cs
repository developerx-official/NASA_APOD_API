using NASA_APOD;

namespace NASA_APOD_EXAMPLE
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
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
        }
    }
}