using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void SetRole(bool isServer)
    {
        Globals.IsServer = isServer;
    }

    public void SetPortServer(int port)
    {
        Globals.PortServer = port;
    }

    public void SetIPServer(string IP)
    {
        Globals.IPServer = IP;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game Scene");
    }
}