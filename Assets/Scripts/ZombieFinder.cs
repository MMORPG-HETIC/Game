using UnityEngine;
using System.Collections.Generic;

public class ZombieFinder : MonoBehaviour
{ 
    public Dictionary<string, GameObject> Zombies = new Dictionary<string, GameObject>();

    public void RegisterZombie(string id, GameObject zombie)
    {
        Debug.Log("register zombie : " + id);
        if (Zombies.ContainsKey(id))
        {
            Zombies.Remove(id);
        }
        Zombies.Add(id, zombie);
    }

    public GameObject FindZombieByID(string id)
    {
        if (Zombies.ContainsKey(id))
        {
            return Zombies[id];
        }

        return null;
    }

    public GameObject FindZombieByPlayerID(string playerId)
    {
        if (Zombies.ContainsKey(playerId))
        {
            return Zombies[playerId];
        }

        return null;
    }

    public IEnumerable<GameObject> GetAllZombies()
    {
        return Zombies.Values;
    }

    public void RemoveZombie(string id)
    {
        GameObject zombie = FindZombieByID(id);
        Zombies.Remove(id);
        //Destroy(zombie);
        DestroyImmediate(zombie);
    }
}
