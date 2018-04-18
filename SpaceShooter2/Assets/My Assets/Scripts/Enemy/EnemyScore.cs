using System;
using TMPro;
using UnityEngine;

namespace Kontrol
{
    public class EnemyScore : MonoBehaviour
    {
        public int enemyScore;
        public Animator scoreAnim;
        private TextMeshProUGUI scoreText;
        private GameObject GM;

        void Start()
        {
            GM = GameObject.Find("GameManager (bring to all scenes)").gameObject;
        }

        public void AddScore()
        {
            GM.GetComponent<ScoreManager>().scoreLevel[RestartScene.currentScene - 1] += enemyScore;
            scoreText = GameObject.Find("Canvas").transform.Find("Score Panel BG").transform.Find("Score Panel").transform.Find("ScoreText").gameObject.GetComponent<TextMeshProUGUI>();
            scoreAnim = scoreText.GetComponent<Animator>();
            scoreAnim.Play("ScorePop");
            scoreText.text = Convert.ToString(GM.GetComponent<ScoreManager>().TotalScore());
        }
    }
}
