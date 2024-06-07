namespace Healthcare.Application.DTOs.Result;

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }

    private Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Failure(string error) => new Result(false, error);
    public static Result Success() => new Result(true, string.Empty);
}