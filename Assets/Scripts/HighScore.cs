using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private Text highScore;

    void Start(){
        highScore = gameObject.GetComponent<Text>();
        highScore.text = DataManager.instance.highScore.ToString();
    }
} 