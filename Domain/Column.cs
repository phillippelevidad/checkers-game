using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Column : ValueObject
    {
        private const int LetterToNumberFactor = 64;

        public const char MaxColumnLetter = 'H';
        public const char MinColumnLetter = 'A';

        public const int MaxColumnNumber = 8;
        public const int MinColumnNumber = 1;

        public Column(char letter)
        {
            Guard.Against.OutOfRange(letter, nameof(letter), MinColumnLetter, MaxColumnLetter);
            Letter = letter;
            Number = Letter - LetterToNumberFactor;
        }

        public char Letter { get; }

        public int Number { get; }

        public static Result<Column> Try(int number)
        {
            try
            {
                var letter = (char)(number + LetterToNumberFactor);
                return new Column(letter);
            }
            catch (ArgumentOutOfRangeException)
            {
                return Result<Column>.Failure($"{number} is not a valid column number.");
            }
        }

        public Result<Column> GetNext()
        {
            char nextLetter = (char)(Letter + 1);
            if (nextLetter > MaxColumnLetter)
            {
                return Result<Column>.Failure($"{nextLetter} is not a valid column.");
            }

            return new Column(nextLetter);
        }

        public Result<Column> GetPrevious()
        {
            char previousLetter = (char)(Number - 1);
            if (previousLetter < MinColumnLetter)
            {
                return Result<Column>.Failure($"{previousLetter} is not a valid column.");
            }

            return new Column(previousLetter);
        }

        public override string ToString()
        {
            return Letter.ToString();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Number;
        }

        public static explicit operator Column(char letter) => new(letter);

        public static explicit operator Column(int number) => Try(number).Value!;
    }
}
