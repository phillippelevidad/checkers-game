using System;

namespace Domain
{
    public class Result<T> : Result
    {
        private readonly T? value;

        protected Result(bool isSuccess, string? error, T? value)
            : base(isSuccess, error)
        {
            this.value = value;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, null, value);
        }

        public static new Result<T> Failure(string error)
        {
            return new Result<T>(false, error, default);
        }

        public T? Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException("Tried to access the Value of a failed result.");
                }

                return value;
            }
        }

        public override string ToString()
        {
            return IsSuccess
                ? $"[Success] {Value}"
                : $"[Failure] {Error}";
        }

        public static implicit operator Result<T>(T value) => Result<T>.Success(value);
    }
}
