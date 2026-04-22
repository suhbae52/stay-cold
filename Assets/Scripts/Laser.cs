using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // referencing scripts.
    private PlayerScript playerScript;

    // referencing game objects.
    private GameObject player;

    //laser stats
    private int damage = 50;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            playerScript.TakeDamage(damage);
        }
    }
}