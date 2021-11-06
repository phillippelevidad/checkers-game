namespace Domain
{
    public class Score
    {
        public Score(int player1Score, int player2Score)
        {
            Player1Score = player1Score;
            Player2Score = player2Score;
        }

        public int Player1Score { get; }

        public int Player2Score { get; }

        public bool Player1Wins => Player1Score == Game.PawnsPerPlayer;

        public bool Player2Wins => Player2Score == Game.PawnsPerPlayer;
    }
}
