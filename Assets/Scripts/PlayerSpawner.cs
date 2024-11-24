using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPoint;


    private void Awake()
    {
        if (!Globals.IsServer)
        {
            gameObject.SetActive(false);
        }
    }

    public void SpawnPlayer()
    {
        GameObject o = Instantiate(player, spawnPoint.position, spawnPoint.rotation);
        Camera camera = player.GetComponent<Camera>();
        camera.enabled = true;
    }
}
