using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint; 
    public GameObject bulletPrefab; 
    public float bulletSpeed = 10f;

    public AudioClip shootSound;
    public float shootVolume = 0.5f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public int totalAmmo = 30;
    public float reloadTime = 2f;

    private AudioSource audioSource;
    private bool isReloading = false;

    void Start()
    {
        // Initialiser les munitions et configurer l'AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = shootVolume;
        }
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // Empêcher de tirer pendant le rechargement
        if (isReloading) return;

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        // Créer et tirer une balle
        currentAmmo--;

        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private IEnumerator Reload()
    {
        // Recharger les munitions
        if (totalAmmo <= 0) yield break;

        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;
        if (totalAmmo >= ammoNeeded)
        {
            currentAmmo = maxAmmo;
            totalAmmo -= ammoNeeded;
        }
        else
        {
            currentAmmo += totalAmmo;
            totalAmmo = 0;
        }

        isReloading = false;
    }
}