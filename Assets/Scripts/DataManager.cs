using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    public static DataManager instance;
    
    public int speedLevel;
    public int healthLevel;
    public int energyLevel;
    public int slowMotionLevel;
    public int scoreMultiplierLevel;
    
    public float speed;
    public float health;
    public float energy;
    public float slowMotion;
    public float scoreMultiplier;
    
    public int coins;
    public int highScore;
    
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameData();
        }
        else {
            Destroy(gameObject);
        }
    }

    public static void Initialize() {
        if (instance == null) {
            GameObject dataManagerObject = new GameObject("DataManager");
            instance = dataManagerObject.AddComponent<DataManager>();
            DontDestroyOnLoad(dataManagerObject);
        }
    }

    private void SaveGameData() {
        SaveSystem.SaveData(this);
    }

    private void LoadGameData() {
        PlayerData data = SaveSystem.LoadData();
        
        speedLevel = data.speedLevel;
        healthLevel = data.healthLevel;
        energyLevel = data.energyLevel;
        slowMotionLevel = data.slowMotionLevel;
        scoreMultiplierLevel = data.scoreMultiplierLevel;
        
        speed = data.speed;
        health = data.health;
        energy = data.energy;
        slowMotion = data.slowMotion;
        scoreMultiplier = data.scoreMultiplier;
        
        coins = data.coins;
        highScore = data.highScore;
    }

    void OnApplicationQuit() {
        SaveGameData();
     }
}
