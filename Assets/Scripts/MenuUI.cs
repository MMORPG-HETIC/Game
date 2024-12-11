using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public TMPro.TMP_InputField InpIP;
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
        Globals.IPServer = InpIP.text ?? "127.0.0.1";
        SceneManager.LoadScene("Game Scene");
    }
}