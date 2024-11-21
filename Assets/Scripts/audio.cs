using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Référence au bouton pour activer/désactiver le son
    public Button soundToggleButton;

    // Booléen pour suivre l'état actuel du son (activé ou désactivé)
    private bool isSoundEnabled = true;

    // Images pour les deux états du bouton
    public Sprite muteImage;
    public Sprite unmuteImage;

    void Start()
    {
        // Ajouter une fonction à exécuter lorsque le bouton est cliqué
        soundToggleButton.onClick.AddListener(ToggleSound);
    }

    void ToggleSound()
    {
        // Inverser l'état du son
        isSoundEnabled = !isSoundEnabled;

        // Activer ou désactiver tous les AudioListeners dans la scène
        AudioListener.pause = !isSoundEnabled;

        // Changer l'image du bouton en fonction de l'état actuel du son
        if (isSoundEnabled)
        {
            soundToggleButton.image.sprite = muteImage;
        }
        else
        {
            soundToggleButton.image.sprite = unmuteImage;
        }
    }
}