using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    public AudioClip shootSound;
    public float shootVolume = 0.5f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = shootVolume;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
