using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private const long Address = 0b0L;
    public GameObject player;
    public Transform spawnPoint;


    //private void Awake()
    //{
    //    if (!Globals.IsServer)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    public void SpawnPlayer(string addr, bool isMe)
    {
        GameObject o = Instantiate(player, spawnPoint.position, spawnPoint.rotation);
        Camera camera = o.GetComponent<Camera>();
        if (camera != null && isMe)
        {
            camera.enabled = true;
        }

        PlayerAttribute playerAttribute = o.AddComponent<PlayerAttribute>();
        playerAttribute.ID = addr;

        PlayerControler playerControler = o.GetComponent<PlayerControler>();
        //Bullet bullet = o.GetComponent<Bullet>();

        if (!isMe)
        {
            playerControler.enabled = false;
            //bullet.enabled = false;
        }
    }
}
