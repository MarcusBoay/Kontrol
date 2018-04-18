using UnityEngine;

namespace Kontrol
{
    public class ScoreManager : MonoBehaviour
    {
        //public int scoreTotal;

        public int[] scoreLevel;

        public int TotalScore()
        {
            int scoreTotal = 0;
            for (int i = 0; i < scoreLevel.Length; i++)
            {
                scoreTotal += scoreLevel[i];
            }
            return scoreTotal;
        }
    }
}
