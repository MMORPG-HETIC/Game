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

    public GameObject SpawnPlayer(string addr, bool isMe)
    {
        GameObject o = Instantiate(player, spawnPoint.position, spawnPoint.rotation);
        Transform cameraTransform = o.transform.Find("Main Camera");
        if (cameraTransform != null && isMe)
        {
            cameraTransform.gameObject.SetActive(true);
        }

        PlayerAttribute playerAttribute = o.AddComponent<PlayerAttribute>();
        playerAttribute.ID = addr;

        //Bullet bullet = o.GetComponent<Bullet>();

        if (!isMe)
        {
            PlayerControler playerControler = o.GetComponent<PlayerControler>();
            AnimationController animationController = o.GetComponent<AnimationController>();
            Rigidbody rigidbody = o.GetComponent<Rigidbody>();
            CharacterController characterController = o.GetComponent<CharacterController>();
            playerControler.enabled = false;
            animationController.enabled = false;
            Destroy(rigidbody);
            Destroy(characterController);
            //bullet.enabled = false;
        }

        return o;
    }
}
