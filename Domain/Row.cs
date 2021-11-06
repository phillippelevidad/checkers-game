using Ardalis.GuardClauses;
using System.Collections.Generic;

namespace Domain
{
    public class Row : ValueObject
    {
        public const int MaxRowNumber = 8;
        public const int MinRowNumber = 1;

        public Row(int number)
        {
            Guard.Against.OutOfRange(number, nameof(number), MinRowNumber, MaxRowNumber);
            Number = number;
        }

        public int Number { get; }

        public static Result<Row> Try(int number)
        {
            if (number < MinRowNumber || number > MaxRowNumber)
            {
                return Result<Row>.Failure($"{number} is not a valid row.");
            }

            return new Row(number);
        }

        public Result<Row> GetNext() => Try(Number + 1);

        public Result<Row> GetPrevious() => Try(Number - 1);

        public override string ToString()
        {
            return Number.ToString();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

        public static explicit operator Row(int number) => new(number);
    }
}
