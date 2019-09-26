using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centipede_V1
{
    public class Scoring
    {
        private int playerScore;

        Scoring()
        {
            playerScore = 0;
        }

        public void increaseScore(int enemyPointValue)
        {
            playerScore += enemyPointValue;
        }

        public int getScore()
        {
            return playerScore;
        }
    }
}