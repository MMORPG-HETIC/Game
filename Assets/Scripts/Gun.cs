using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    public AudioClip shootSound; // Le son de tir à jouer
    public float shootVolume = 0.5f; // Le volume du son de tir
    private AudioSource audioSource; // Référence au composant AudioSource

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtenir le composant AudioSource attaché à ce GameObject
        audioSource.volume = shootVolume; // Ajuster le volume du son de tir
    }

    void Update()
    {
        // Vérifier si le joueur appuie sur le bouton droit de la souris
        if (Input.GetMouseButtonDown(1))
        {
            Shoot(); // Effectuer un tir
        }
    }

    void Shoot()
    {
        // Créer une instance du projectile à partir du prefab à la position et rotation du point de spawn du projectile
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Donner une vélocité au projectile pour le faire avancer dans la direction du point de spawn
        bullet.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint.forward * bulletSpeed;

        // Jouer le son de tir
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
