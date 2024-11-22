using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitButton()
    {
        Debug.Log("Game is quitting...");

        // Simulation dans l'Éditeur
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Arrête le mode Play
        #else
        Application.Quit(); // Quitte dans une version buildée
        #endif
    }
}
