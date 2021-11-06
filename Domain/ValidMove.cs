namespace Domain
{
    public class ValidMove
    {
        private ValidMove(Player player, Position fromPosition, Position toPosition, BoardCell? pawnToBeTaken, bool isMandatory, ValidMove? followUpMove)
        {
            Player = player;
            FromPosition = fromPosition;
            ToPosition = toPosition;
            PawnToBeTaken = pawnToBeTaken;
            IsMandatory = isMandatory;
            FollowUpMove = followUpMove;
        }

        public static ValidMove SimpleMove(Player player, Position fromPosition, Position toPosition)
        {
            return new ValidMove(player, fromPosition, toPosition, null, false, null);
        }

        public static ValidMove JumpMove(Player player, Position fromPosition, Position toPosition, BoardCell pawnToBeTaken, ValidMove? followUpMove)
        {
            return new ValidMove(player, fromPosition, toPosition, pawnToBeTaken, true, followUpMove);
        }

        public Player Player { get; }

        public Position FromPosition { get; }

        public Position ToPosition { get; }

        public BoardCell? PawnToBeTaken { get; }

        public bool IsMandatory { get; }

        public ValidMove? FollowUpMove { get; }

        public override string ToString()
        {
            var mandatory = IsMandatory ? ", mandatory" : "";
            var followup = FollowUpMove is not null ? $" > {FollowUpMove}" : "";

            return $"{Player}, {FromPosition} to {ToPosition}{mandatory}{followup}";
        }

        public bool IsMatch(Position fromPosition, Position toPosition)
        {
            return
                FromPosition == fromPosition &&
                ToPosition == toPosition;
        }
    }
}
