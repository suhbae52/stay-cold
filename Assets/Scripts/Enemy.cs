using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // referencing game objects.
    public Transform player;
    public Transform[] enemies;

    // radius of the circle to place the enemies.
    private float radius = 40;
 
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        // setting the enemies apart a certain distance in a circular shape.
        for (int i = 0; i < enemies.Length; i++){
            float angle = i * Mathf.PI * 2f / enemies.Length;
            enemies[i].transform.position = player.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        }
    }
}