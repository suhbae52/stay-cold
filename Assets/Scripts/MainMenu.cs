using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start() {
        AudioManager.instance.Play("Main Menu Sound");
    }
    public void PlayGame(){
        AudioManager.instance.Stop("Main Menu Sound");
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void QuitGame(){
        Application.Quit();
    }
    
    public void PlayButtonSound() {
        AudioManager.instance.Play("Button Sound");
    }

    public void UpdateVolume() {
        AudioManager.instance.UpdateVolume();
    }
}