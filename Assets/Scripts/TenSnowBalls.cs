using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenSnowBalls : MonoBehaviour
{
    private LevelManager levelManagerScript;
    
    void Start() {
        levelManagerScript = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            FindObjectOfType<AudioManager>().Play("Ten Snow Ball Sound");
            levelManagerScript.AddScore(100f);
            levelManagerScript.AddSnowBall(10);
            Destroy(gameObject);
        }
    }
}
