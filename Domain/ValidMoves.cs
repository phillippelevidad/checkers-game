using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class ValidMoves : IEnumerable<ValidMove>
    {
        private readonly IReadOnlyList<ValidMove> validMoves;

        public ValidMoves(IEnumerable<ValidMove> validMoves)
        {
            this.validMoves = validMoves.ToList().AsReadOnly();
        }

        public IReadOnlyList<ValidMove> Entries => validMoves;

        public Result<ValidMove> Find(Position fromPosition, Position toPosition)
        {
            if (validMoves.Any(move => move.IsMandatory))
            {
                var mandatoryMatch = validMoves.FirstOrDefault(move =>
                    move.IsMandatory &&
                    move.IsMatch(fromPosition, toPosition));

                if (mandatoryMatch is null)
                {
                    return Result<ValidMove>.Failure($"{fromPosition} to {toPosition} is not one of the mandatory moves.");
                }

                return mandatoryMatch;
            }
            else
            {
                var match = validMoves.FirstOrDefault(move =>
                    move.IsMatch(fromPosition, toPosition));

                if (match is null)
                {
                    return Result<ValidMove>.Failure($"{fromPosition} to {toPosition} is not a valid move.");
                }

                return match;
            }
        }

        public IEnumerator<ValidMove> GetEnumerator()
        {
            return validMoves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)validMoves).GetEnumerator();
        }
    }
}
