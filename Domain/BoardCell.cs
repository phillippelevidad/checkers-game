namespace Domain
{
    public class BoardCell
    {
        public BoardCell(Position position)
        {
            Position = position;
        }

        public Position Position { get; }

        public Pawn? Pawn { get; private set; }

        public bool IsFree => Pawn is null;

        public bool HasPawn => Pawn is not null;

        public Result PlacePawn(Pawn pawn)
        {
            if (HasPawn)
            {
                return Result.Failure($"Position {Position} is not free.");
            }

            Pawn = pawn;
            return Result.Success();
        }

        public Result<Pawn> Free()
        {
            if (IsFree)
            {
                return Result<Pawn>.Failure($"Position {Position} has no pawns.");
            }

            var pawn = Pawn;
            Pawn = null;
            return pawn!;
        }

        public bool IsAdversaryKingsRow(Player forPlayer)
        {
            return
                (forPlayer == Player.Player1 && Position.Row.Number == Row.MaxRowNumber) ||
                (forPlayer == Player.Player2 && Position.Row.Number == Row.MinRowNumber);
        }

        public override string ToString()
        {
            var info = HasPawn ? Pawn!.Owner.ToString() : "Free";
            return $"{Position} {info}";
        }
    }
}
