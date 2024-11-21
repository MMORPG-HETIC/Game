using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundClip; // Clip audio de la musique de fond
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Assurez-vous que le clip audio est assigné
        if (backgroundClip != null)
        {
            // Définir le clip audio et configurer la boucle
            audioSource.clip = backgroundClip;
            audioSource.loop = true;

            // Démarrer la lecture de la musique de fond
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Aucun clip audio n'est assigné pour la musique de fond dans le script BackgroundMusic.");
        }
    }
}