
using System.Net.Http.Json;

string apiUrl = args.Length > 0
    ? args[0]
    : "http://localhost:5000";

using var client = new HttpClient
{
    BaseAddress = new Uri(apiUrl)
};

try
{
    Console.WriteLine("Dang lay danh sach bai tap...");

    var assignments =
        await client.GetFromJsonAsync<List<Assignment>>(
            "/api/assignments"
        );

    Console.WriteLine("\nDanh sach bai tap:");

    if (assignments != null)
    {
        foreach (var item in assignments)
        {
            Console.WriteLine(
                $"- {item.Id}: {item.Title}"
            );
        }
    }

    var request = new CreateSubmissionRequest(
        1,
        "SV001",
        "https://github.com/student/EduSubmit"
    );

    Console.WriteLine("\nDang nop bai...");

    var response = await client.PostAsJsonAsync(
        "/api/submissions",
        request
    );

    response.EnsureSuccessStatusCode();

    var result =
        await response.Content.ReadFromJsonAsync<SubmissionResult>();

    if (result != null)
    {
        Console.WriteLine($"Ma luot nop: {result.Id}");
        Console.WriteLine($"Trang thai: {result.Status}");
    }
}
catch (HttpRequestException ex)
{
    Console.WriteLine("Loi ket noi API: " + ex.Message);
}

record Assignment(int Id, string Title, string Description);

record CreateSubmissionRequest(
    int AssignmentId,
    string StudentCode,
    string RepositoryUrl
);

record SubmissionResult(
    Guid Id,
    int AssignmentId,
    string StudentCode,
    string Status,
    DateTimeOffset ReceivedAt
);
