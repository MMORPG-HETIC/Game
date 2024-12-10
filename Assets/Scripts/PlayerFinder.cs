using UnityEngine;
using System.Collections.Generic;
public class PlayerFinder : MonoBehaviour
{
    public Dictionary<string, GameObject> Players = new Dictionary<string, GameObject>();

    public void RegisterPlayer (string id, GameObject player)
    {
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
}
