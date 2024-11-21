using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // prefab du zombie à faire apparaître
    public Transform spawnPoint;    // point où le zombie doit apparaître
    public float delay = 10f;       // Temps avant l'apparition

    private int zombieID = 0; //  attribuer un ID unique à chaque zombie

    void Start()
    {
        // Faire apparaître le zombie après un délai
        Invoke("SpawnZombie", delay);
    }

    void SpawnZombie()
    {
        // Instancier le zombie au point défini
        GameObject newZombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        // Attribuer un ID unique en tant que nom du zombie
        zombieID++;
        newZombie.name = "Zombie_" + zombieID;

        Debug.Log("Zombie " + zombieID + " spawné !");
    }
}
