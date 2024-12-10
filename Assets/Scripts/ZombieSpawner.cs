using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform spawnPoint;

    public GameObject SpawnZombie(string addr)
    {
        GameObject o = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        ZombieAttribute zombieAttribute = o.AddComponent<ZombieAttribute>();
        zombieAttribute.ID = addr;

        return o;
    }
}
