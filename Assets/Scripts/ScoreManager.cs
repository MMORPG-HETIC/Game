using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0; // Variable pour stocker le score
    public Text scoreText; // Référence au texte affichant le score

    void Start()
    {
        UpdateScoreText(); // Mettre à jour le texte du score au début du jeu
    }

    public void IncrementScore()
    {
        score++; // Incrémenter le score
        UpdateScoreText(); // Mettre à jour le texte du score
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString(); // Mettre à jour le texte du score affiché à l'écran
    }
}
