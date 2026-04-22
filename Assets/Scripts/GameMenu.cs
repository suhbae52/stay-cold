using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void MainMenu() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        AudioManager.instance.StopAll();
    }
}
