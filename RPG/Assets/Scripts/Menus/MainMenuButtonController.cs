using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Toy Box");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
