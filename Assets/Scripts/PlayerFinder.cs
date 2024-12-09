using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    public GameObject FindPlayerByIP(string id)
    {
        PlayerAttribute[] allPlayers = FindObjectsByType<PlayerAttribute>(FindObjectsSortMode.None);

        foreach (PlayerAttribute player in allPlayers)
        {
            if (player.ID == id)
            {
                return player.gameObject;
            }
        }

        return null;
    }
}
