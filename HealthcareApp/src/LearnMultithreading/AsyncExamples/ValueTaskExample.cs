using Healthcare.Application.DTOs.Schedule;
using Newtonsoft.Json;

namespace LearnMultithreading.AsyncExamples;

public static class ValueTaskExample
{
    public static async ValueTask<TimeSlotsDictionary> FetchData()
    {
        using var client = new HttpClient();
        const string url =
            "https://localhost:7255/api/Schedule?DoctorId=858cf140-b2ed-4533-8169-89e6dc3d735f&Pagesize=5";

        try
        {
            var result = await client.GetAsync(url);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request failed with status code {result.StatusCode}");
            }

            var content = await result.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<dynamic>(content);

            if (resp == null || resp.body == null)
            {
                throw new JsonException("Invalid JSON structure: 'body' property is missing");
            }

            var body = resp.body.ToString();
            var timeSlots = JsonConvert.DeserializeObject<TimeSlotsDictionary>(body);

            if (timeSlots == null)
            {
                throw new JsonException("Deserialization to TimeSlotsDictionary failed");
            }

            return timeSlots;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }

    }
}