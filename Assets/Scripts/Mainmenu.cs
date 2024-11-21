using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
