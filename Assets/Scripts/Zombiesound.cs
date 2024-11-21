using UnityEngine;

public class ZombieSound : MonoBehaviour
{
    public AudioClip detectionSound; // Son à jouer lors de la détection du joueur
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Assurez-vous que le clip audio est assigné
        if (detectionSound == null)
        {
            Debug.LogWarning("Aucun clip audio n'est assigné pour le son de détection du zombie dans le script ZombieSound.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Vérifie si le zombie entre en collision avec le joueur
        if (other.CompareTag("Player"))
        {
            // Jouer le son de détection
            PlayDetectionSound();
        }
    }

    void PlayDetectionSound()
    {
        if (detectionSound != null && !audioSource.isPlaying)
        {
            // Définir le clip audio et jouer le son
            audioSource.clip = detectionSound;
            audioSource.Play();
        }
    }
}