using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

public class ShopManager : MonoBehaviour {
    
    public TextMeshProUGUI totalCoins;

    [System.Serializable]
    public class Upgrade {
        public string name;
        public int level;
        public int levelMax;
        public float minValue;
        public float maxValue;
        public int price;
        public TextMeshProUGUI priceText;
        public Slider slider;
        public TextMeshProUGUI progressText;
    }

    public List<Upgrade> upgrades = new List<Upgrade>();
    private const int BasePrice = 400;
    
    // score multiplier upgrade
    public TextMeshProUGUI scoreMultiplierPriceText;
    public Slider scoreMultiplierSlider;
    public TextMeshProUGUI scoreMultiplierProgressText;
    private const int ScoreMultiplierPrice = 1000;
    private const int ScoreMultiplierLevelMax = 100;
    
    void Start() {
        totalCoins.text = DataManager.instance.coins.ToString();
        InitializeUpgrades();
    }

    private void InitializeUpgrades() {
        foreach (var upgrade in upgrades) {
            upgrade.level = GetLevelFromDataManager(upgrade.name);
            upgrade.price = CalculatePrice(upgrade.level);
            UpdateUI(upgrade);
        }
        UpdateScoreMultiplierUI();
    }

    public void UpgradeScoreMultiplier() {
        if (DataManager.instance.scoreMultiplierLevel < ScoreMultiplierLevelMax) {
            if (DataManager.instance.coins >= ScoreMultiplierPrice) {
                DataManager.instance.coins -= ScoreMultiplierPrice;
                UpdateCoins();
                DataManager.instance.scoreMultiplierLevel++;
                float level = DataManager.instance.scoreMultiplierLevel;
                DataManager.instance.scoreMultiplier = (1 + (level * 0.1f));
                UpdateScoreMultiplierUI();
            }
            else {
                Debug.Log("Not enough coins");
            }
        }
    }
    
    public void UpgradeStat(int upgradeIndex) {
        // for error checking
        if (upgradeIndex < 0 || upgradeIndex >= upgrades.Count) return;
        var thisUpgrade = upgrades[upgradeIndex];
        if (thisUpgrade.level < thisUpgrade.levelMax) {
            int price = CalculatePrice(thisUpgrade.level);
            if (DataManager.instance.coins >= price) {
                DataManager.instance.coins -= price;
                UpdateCoins();
                thisUpgrade.level++;
                ApplyStatToDataManager(thisUpgrade);
                thisUpgrade.price = CalculatePrice(thisUpgrade.level);
                UpdateUI(thisUpgrade);
            }
            else {
                // maybe change in the future to something else (grey out the button)
                Debug.Log("Not enough coins");
            }
        }
    }

    private void UpdateUI(Upgrade upgrade) {
        if (upgrade.priceText != null)
            upgrade.priceText.text = upgrade.level < upgrade.levelMax
                ? CalculatePrice(upgrade.level).ToString()
                : "MAX";
        if (upgrade.slider != null)
            SliderUpdate(upgrade.slider, upgrade.level, upgrade.levelMax);
        if (upgrade.progressText != null) {
            upgrade.progressText.text = 
                "LEVEL " + upgrade.level + ": " + 
                CalculateStatValue(upgrade.level, upgrade.levelMax, upgrade.minValue, upgrade.maxValue);
        }
    }

    private void UpdateScoreMultiplierUI() {
        if (scoreMultiplierPriceText != null)
            scoreMultiplierPriceText.text = DataManager.instance.scoreMultiplierLevel < ScoreMultiplierLevelMax
                ? ScoreMultiplierPrice.ToString()
                : "MAX";
        if (scoreMultiplierSlider != null)
            scoreMultiplierSlider.value = (float)DataManager.instance.scoreMultiplierLevel / ScoreMultiplierLevelMax;
        if (scoreMultiplierProgressText.text != null)
            scoreMultiplierProgressText.text =
                "LEVEL " + DataManager.instance.scoreMultiplierLevel + ": x" + DataManager.instance.scoreMultiplier;
    }

    private int CalculatePrice(int level) {
        return (level + 1) * BasePrice;
    }

    private float CalculateStatValue(int level, int levelMax, float minValue, float maxValue) {
        return Mathf.Lerp(minValue, maxValue, (float)level / levelMax);
    }

    private void SliderUpdate(Slider slider, int level, int levelMax) {
        slider.value = (float)level / levelMax;
    }

    private void ApplyStatToDataManager(Upgrade upgrade) {
        float newStatValue = CalculateStatValue(upgrade.level, upgrade.levelMax, upgrade.minValue, upgrade.maxValue);
        SetStatInDataManager(upgrade.name, newStatValue, upgrade.level);
    }

    private void SetStatInDataManager(string statName, float value, int level) {
        switch (statName) {
            case "Speed":
                DataManager.instance.speed = value;
                DataManager.instance.speedLevel = level;
                break;
            case "Health": 
                DataManager.instance.health = value;
                DataManager.instance.healthLevel = level;
                break;
            case "Energy": 
                DataManager.instance.energy = value;
                DataManager.instance.energyLevel = level;
                break;
            case "Slow Motion": 
                DataManager.instance.slowMotion = value;
                DataManager.instance.slowMotionLevel = level;
                break;
        }
    }

    private int GetLevelFromDataManager(string statName) {
        return statName switch {
            "Speed" => DataManager.instance.speedLevel,
            "Health" => DataManager.instance.healthLevel,
            "Energy" => DataManager.instance.energyLevel,
            "Slow Motion" => DataManager.instance.slowMotionLevel,
            _ => 1,
        };
    }

    public void ResetStats()
    {
        DataManager.instance.speedLevel = 0;
        DataManager.instance.healthLevel = 0;
        DataManager.instance.energyLevel = 0;
        DataManager.instance.slowMotionLevel = 0;
        DataManager.instance.scoreMultiplierLevel = 0;

        DataManager.instance.speed = 10f;
        DataManager.instance.health = 100f;
        DataManager.instance.energy = 100f;
        DataManager.instance.slowMotion = 2f;
        DataManager.instance.scoreMultiplier = 1f;
        
        DataManager.instance.coins = 0;
        
        InitializeUpgrades();
        UpdateCoins();
    }

    public void AddCoins()
    {
        DataManager.instance.coins += 1000000;
        UpdateCoins();
    }

    private void UpdateCoins() {
        totalCoins.text = DataManager.instance.coins.ToString();
    }
}