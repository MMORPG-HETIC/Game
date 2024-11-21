using UnityEngine;

public class Zombie : MonoBehaviour
{
    private int zombieID;

    // Méthode pour définir l'ID du zombie
    public void SetID(int id)
    {
        zombieID = id;
        Debug.Log($"Zombie ID: {zombieID} initialisé.");
    }

    // Méthode pour récupérer l'ID du zombie 
    public int GetID()
    {
        return zombieID;
    }
}
