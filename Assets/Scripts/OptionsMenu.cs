using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public GameObject fixedConfirm;
    public GameObject floatingConfirm;
    
    public Slider masterSlider;
    public Slider musicSlider;
    
    private void Start() {
        masterSlider.value = PlayerPrefs.GetFloat("Master Volume");
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume");
        
        masterSlider.onValueChanged.AddListener(delegate { UpdateMasterVolume(); });
        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });

        CheckJoyStick();
    }

    private void CheckJoyStick() {
        if (PlayerPrefs.GetInt("Joystick Type") == 1) {
            SetToFixed();
        }
        else {
            SetToFloating();
        }
    }
    
    public void SetToFixed() {
        PlayerPrefs.SetInt("Joystick Type", 1);
        fixedConfirm.SetActive(true);
        floatingConfirm.SetActive(false);
    }

    public void SetToFloating() {
        PlayerPrefs.SetInt("Joystick Type", 2);
        fixedConfirm.SetActive(false);
        floatingConfirm.SetActive(true);
    }

    private void UpdateMasterVolume() {
        PlayerPrefs.SetFloat("Master Volume", masterSlider.value);
    }

    private void UpdateMusicVolume() {
        PlayerPrefs.SetFloat("Music Volume", musicSlider.value);
    }
}