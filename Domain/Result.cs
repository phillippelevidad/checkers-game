using Ardalis.GuardClauses;

namespace Domain
{
    public class Result
    {
        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Failure(string error)
        {
            Guard.Against.NullOrWhiteSpace(error, nameof(error), "Provide a reason for the failure.");
            return new Result(false, error);
        }

        public bool IsSuccess { get; }

        public string? Error { get; }

        public bool IsFailure => !IsSuccess;

        public override string ToString()
        {
            return IsSuccess
                ? "[Success]"
                : $"[Failure] {Error}";
        }
    }
}
