using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform spawnPoint;


    private void Awake()
    {
        if (!Globals.IsServer)
        {
            gameObject.SetActive(false);
        }
    }

    public void SpawnZombie()
    {
        Invoke("SpawnZombie", Time.time);
        GameObject o = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        //Zombie z = o.AddComponent<Zombie>();
    }
}
