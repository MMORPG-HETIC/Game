using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPoint;

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
            Gun gun = o.GetComponent<Gun>();
            Bullet bullet = o.GetComponent<Bullet>();
            gun.enabled = false;
            bullet.enabled = false;
            playerControler.enabled = false;
            animationController.enabled = false;
            Destroy(rigidbody);
            Destroy(characterController);
            //bullet.enabled = false;
        }

        return o;
    }
}
