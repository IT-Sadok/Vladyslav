using Newtonsoft.Json;

namespace LearnMultithreading.Operations;

public static class IOBoundOperations
{
    public static async Task<object> FetchData()
    {
        using var client = new HttpClient();
        const string url =
            "https://api.monobank.ua/bank/currency";
        var result = await client.GetAsync(url);

        var array = JsonConvert.DeserializeObject<object>(await result.Content.ReadAsStringAsync());
        return array;
    }
}