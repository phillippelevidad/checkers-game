namespace Domain
{
    public record Pawn
    {
        public Pawn(Player owner)
        {
            Owner = owner;
        }

        public Player Owner { get; }

        public bool IsCrowned { get; private set; }

        public void Crown()
        {
            IsCrowned = true;
        }

        public override string ToString()
        {
            return Owner.ToString();
        }
    }
}
