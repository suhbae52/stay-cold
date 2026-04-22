using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    private LevelManager levelManagerScript;
    
    void Start() {
        levelManagerScript = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            FindObjectOfType<AudioManager>().Play("Snow Ball Sound");
            levelManagerScript.AddScore(50f);
            levelManagerScript.AddSnowBall(1);
            Destroy(gameObject);
        }
    }
}
