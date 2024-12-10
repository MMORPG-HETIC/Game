using UnityEngine;
using System.Collections.Generic;
public class PlayerFinder : MonoBehaviour
{
    public Dictionary<string, GameObject> Players = new Dictionary<string, GameObject>();

    public void RegisterPlayer(string id, GameObject player)
    {
        Debug.Log("register player : " + id);
        Players.Add(id, player);
    }

    public GameObject FindPlayerByID(string id)
    {
        if (Players.ContainsKey(id))
        {
            return Players[id];
        }

        return null;
    }

    public void RemovePlayer(string id)
    {
        GameObject player = FindPlayerByID(id);
        Players.Remove(id);
        Destroy(player);
    }
}
