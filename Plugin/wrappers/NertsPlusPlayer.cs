using System.Collections.Generic;

namespace NertsPlus
{
    public class NertsPlusPlayer
    {
        private Player player;

        public NertsPlusPlayer(Player player)
        {
            this.player = player;
        }

        public int GetScore()
        {
            return player.int_0;
        }

        public void SetScore(int score)
        {
            player.int_0 = score;
        }

        public List<Card> GetNertsPile()
        {
            return player.list_1;
        }

        public List<sbyte> GetScores()
        {
            return player.list_4;
        }
    }
}
