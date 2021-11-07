using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain
{
    public class Position : ValueObject
    {
        public Position(Row row, Column column)
        {
            Name = $"{row.Number}{column.Letter}";
            if (!IsValidCombination(row, column))
            {
                throw new ArgumentOutOfRangeException(nameof(row), $"{Name} is not a valid position.");
            }

            Row = row;
            Column = column;
        }

        public Row Row { get; }

        public Column Column { get; }

        public string Name { get; }

        public static Result<Position> FromName(string positionName)
        {
            const string pattern = @"[1-8][A-H]";
            if (!Regex.IsMatch(positionName, pattern, RegexOptions.IgnoreCase))
            {
                return Result<Position>.Failure($"'{positionName}' is not a valid position.");
            }

            positionName = positionName.ToUpper();
            var rowNumber = int.Parse(positionName[0].ToString());
            var columnLetter = positionName[1];

            return new Position((Row)rowNumber, (Column)columnLetter);
        }

        public static Result<Position> Try(Row row, Column column)
        {
            try
            {
                return new Position(row, column);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Result<Position>.Failure(ex.Message);
            }
        }

        public static Result<Position> Try(int row, int column)
        {
            var rowResult = Row.Try(row);
            var columnResult = Column.Try(column);

            if (rowResult.IsFailure || columnResult.IsFailure)
            {
                return Result<Position>.Failure($"Row {row} x Column {column} is not a valid position.");
            }

            return Try(rowResult.Value!, columnResult.Value!);
        }

        public override string ToString()
        {
            return Name;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }

        public static explicit operator Position(string positionName) => FromName(positionName).Value!;

        private static bool IsValidCombination(Row row, Column column)
        {
            var isEvenRow = row.Number % 2 == 0;
            var isEvenColumn = column.Number % 2 == 0;
            return isEvenRow == isEvenColumn;
        }
    }
}
