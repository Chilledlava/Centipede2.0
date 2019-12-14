using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centipede_V1
{
    static public class Scoring
    {
        static private int playerScore = 0;
        

        static public void increaseScore(int enemyPointValue)
        {
            playerScore += enemyPointValue;
           

        }

        static public int getScore()
        {
            return playerScore;
            
        }
        
    }
}