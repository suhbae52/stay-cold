using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // referencing scripts.
    private BarrelScript barrelScript;

    // referencing game objects.
    public GameObject gameOverPanel;
    public GameObject[] barrel;

    // game managing variables.
    public bool gameOver;
    public int phase;
    private bool isPaused;

    // slow motion.
    public bool slowMo;
    private float timeSlowMultiplier;

    // game score.
    public float totalScore;
    private float displayScore;
    public float phaseScoreMultiplier = 1f;
    public TextMeshProUGUI scoreText;
    private float maxScore = 99999999999f;
    
    // snow balls.
    public int totalSnowBalls;
    private float displaySnowBalls;
    public TextMeshProUGUI snowBallText;

    void Awake() {
        Application.targetFrameRate = 120;
    }
    
    void Start(){
        AudioManager.instance.Play("Music");
        barrel = GameObject.FindGameObjectsWithTag("Barrel");
        StartCoroutine(ScoreUpdater());
        StartCoroutine(SnowBallUpdater());
        StartCoroutine(PhaseManager());
        timeSlowMultiplier = DataManager.instance.slowMotion;
    }

    public void Update() {
        if(!isPaused && !gameOver && totalScore < maxScore) {
            totalScore +=
                (((Time.unscaledDeltaTime * 15) * phaseScoreMultiplier) * DataManager.instance.scoreMultiplier);
        }
        
        scoreText.text = totalScore.ToString("0");
        snowBallText.text = totalSnowBalls.ToString("0");

        if(!slowMo){
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.unscaledDeltaTime * 7.5f);
        }else{
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f / timeSlowMultiplier, Time.unscaledDeltaTime * 7.5f);
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        // float targetTimeScale = slowMo ? 1f / timeSlowMultiplier : 1f;
        // Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * 7.5f);
        // Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    // public void FixedUpdate() {
    //     float targetTimeScale = slowMo ? 1f / timeSlowMultiplier : 1f;
    //     Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * 7.5f);
    //     Time.fixedDeltaTime = Time.timeScale * .02f;
    // }

    // public void FixedUpdate(){
    //     scoreText.text = totalScore.ToString("0");
    //     snowBallText.text = totalSnowBalls.ToString("0");
    //
    //     if(!slowMo){
    //         Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.unscaledDeltaTime * 7.5f);
    //     }else{
    //         Time.timeScale = Mathf.Lerp(Time.timeScale, 1f / timeSlowMultiplier, Time.unscaledDeltaTime * 7.5f);
    //         Time.fixedDeltaTime = Time.timeScale * .02f;
    //     }
    // }
    public void PlayButtonSound() {
        AudioManager.instance.Play("Button Sound");
    }

    public void Pause() {
        isPaused = true;
        Time.timeScale = 0f;
        AudioManager.instance.UpdateVolume();
        AudioManager.instance.PauseAll();
    }

    public void Resume() {
        isPaused = false;
        Time.timeScale = 1f;
        AudioManager.instance.UpdateVolume();
        AudioManager.instance.ResumeAll();
    }
    
    public void AddScore(float add){
        if(gameOver == false) {
            if (totalScore > maxScore) {
                totalScore = maxScore;
            }
            else {
                totalScore += (add * phaseScoreMultiplier * DataManager.instance.scoreMultiplier);
            }
        }
    }

    private IEnumerator ScoreUpdater()
    {
        while(true){
            if(!isPaused && displayScore < totalScore){
                displayScore++;  
            }
            yield return new WaitForSeconds(0.005f);
        } 
    }

    private IEnumerator SnowBallUpdater() {
        while (true) {
            if (!isPaused && displaySnowBalls < totalSnowBalls) {
                displaySnowBalls++;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
    
    public void AddSnowBall(int amount) {
        totalSnowBalls += amount;
    }
    
    private IEnumerator PhaseManager(){
        for (int i = 0; i < 25; i++) {
            yield return new WaitForSeconds(10f);
            if (gameOver)
                break;
            phase++;
            phaseScoreMultiplier += 0.3f;
            // this for loop will change the phases for all existing barrels.
            for (int j = 0; j < barrel.Length; j++) {
                barrelScript = barrel[j].GetComponent<BarrelScript>();
                barrelScript.UpdatePhase();
            }
        }
    }
    public IEnumerator Die(){ 
        AudioManager.instance.Play("Game Over");
        gameOver = true;
        if(totalScore > DataManager.instance.highScore){
            DataManager.instance.highScore = (int)totalScore;
        }
        DataManager.instance.coins += totalSnowBalls;
        FindObjectOfType<AudioManager>().FadeOut("Music");
        yield return new WaitForSeconds(1f);
        gameOverPanel.SetActive(true);
    }
}