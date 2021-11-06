namespace Domain
{
    public enum Player
    {
        Player1 = 0,
        Player2
    }

    public static class PlayerExtensions
    {
        public static Player GetAdversary(this Player player)
        {
            return player == Player.Player1 ? Player.Player2 : Player.Player1;
        }
    }
}
