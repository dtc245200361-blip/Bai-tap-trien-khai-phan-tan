
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Danh sach bai tap
var assignments = new[]
{
    new Assignment(1, "Bai tap 1", "Tao API dau tien"),
    new Assignment(2, "Bai tap 2", "Xay dung he thong phan tan")
};

// GET: Kiem tra API
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        service = "Submission.Api",
        status = "running"
    });
});

// GET: Lay danh sach bai tap
app.MapGet("/api/assignments", () =>
{
    return Results.Ok(assignments);
});

// POST: Nop bai tap
app.MapPost("/api/submissions", (CreateSubmissionRequest request) =>
{
    if (request.AssignmentId <= 0 ||
        string.IsNullOrWhiteSpace(request.StudentCode) ||
        string.IsNullOrWhiteSpace(request.RepositoryUrl))
    {
        return Results.BadRequest(new
        {
            message = "Du lieu nop bai khong hop le."
        });
    }

    var result = new SubmissionResult(
        Guid.NewGuid(),
        request.AssignmentId,
        request.StudentCode,
        "received",
        DateTimeOffset.UtcNow
    );

    return Results.Created(
        $"/api/submissions/{result.Id}",
        result
    );
});

app.Run();

// Cac model du lieu
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
