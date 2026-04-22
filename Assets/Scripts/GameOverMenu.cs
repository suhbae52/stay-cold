using TMPro;
using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    public LevelManager levelManagerScript;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI snowBallText;
    
    public float totalScore;
    public float displayScore;
    
    public int totalSnowBalls;
    public float displaySnowBalls;

    private const float duration = 1f;
    
    void Start() {
        totalScore = levelManagerScript.totalScore;
        totalSnowBalls = levelManagerScript.totalSnowBalls;
        
        StartCoroutine(UpdateScoreOverTime());
        StartCoroutine(UpdateSnowBallsOverTime());
    }

    void Update() {
        scoreText.text = displayScore.ToString("0");
        snowBallText.text = displaySnowBalls.ToString("0");
    }
    
    private IEnumerator UpdateScoreOverTime() {
        float elapsed = 0f;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            displayScore = Mathf.Lerp(0, totalScore, elapsed / duration);
            yield return null; // Wait for the next frame
        }
        displayScore = totalScore; // Ensure it ends at the exact total
    }

    private IEnumerator UpdateSnowBallsOverTime() {
        float elapsed = 0f;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            displaySnowBalls = Mathf.Lerp(0, totalSnowBalls, elapsed / duration);
            yield return null; // Wait for the next frame
        }
        displaySnowBalls = totalSnowBalls; // Ensure it ends at the exact total
    }
}
