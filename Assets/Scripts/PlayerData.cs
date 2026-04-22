[System.Serializable]
public class PlayerData{

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

    public PlayerData(DataManager dataManager)
    {
        speedLevel = dataManager.speedLevel;
        healthLevel = dataManager.healthLevel;
        energyLevel = dataManager.energyLevel;
        slowMotionLevel = dataManager.slowMotionLevel;
        scoreMultiplierLevel = dataManager.scoreMultiplierLevel;
        
        speed = dataManager.speed;
        health = dataManager.health;
        energy = dataManager.energy;
        slowMotion = dataManager.slowMotion;
        scoreMultiplier = dataManager.scoreMultiplier;
        
        coins = dataManager.coins;
        highScore = dataManager.highScore;
    }

    public PlayerData()
    {
        speedLevel = 0;
        healthLevel = 0;
        energyLevel = 0;
        slowMotionLevel = 0;
        scoreMultiplierLevel = 0;

        speed = 10f;
        health = 100f;
        energy = 100f;
        slowMotion = 2f;
        scoreMultiplier = 1f;

        coins = 0;
        highScore = 0;
    }
}
