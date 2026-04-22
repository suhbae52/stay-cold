using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ItemSpawner : MonoBehaviour {
    // referencing game objects.
    public GameObject ice;
    public GameObject dryIce;
    public GameObject shockWaveItem;
    public GameObject snowBall;
    public GameObject tenSnowBalls;
    public GameObject player;

    public LevelManager levelManagerScript;

    public List<Vector3> restrictedAreas = new List<Vector3>();


    private void Start() {
         //setting some restricted areas for the branches
         restrictedAreas.Add(new Vector3(-6, 26, 0)); //top middle
         restrictedAreas.Add(new Vector3(18, 18, 0)); //top right
         restrictedAreas.Add(new Vector3(30, 6, 0)); //middle right 
         restrictedAreas.Add(new Vector3(-14, 6, 0)); //middle left
         restrictedAreas.Add(new Vector3(22.5f, -22.5f, 0)); //bottom right

        // spawning items.
        StartCoroutine(Spawn(ice, 30f, 60f, 45f, 60f));
        StartCoroutine(Spawn(dryIce, 30f, 60f, 30f, 45f));
        StartCoroutine(Spawn(shockWaveItem, 60f, 75f, 60f, 90f));
        StartCoroutine(Spawn(snowBall, 5f, 7f, 0f, 2f));
    }

    private IEnumerator Spawn(GameObject item, float initialDelayMin, float initialDelayMax, float intervalMin, float intervalMax) {
        var initialDelay = Random.Range(initialDelayMin, initialDelayMax);
        yield return new WaitForSeconds(initialDelay);

        while (!levelManagerScript.gameOver) {
            if (item == snowBall) {
                int snowBallCount = CountSnowBalls();
                if (snowBallCount >= 40) {
                    yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
                    continue;
                }
            }
            
            SpawnItem(item);
            var randomInterval = Random.Range(intervalMin, intervalMax);
            yield return new WaitForSeconds(randomInterval);
        }
    }
    
    private void SpawnItem(GameObject item) {
        Vector3 spawnPos;
        int maxAttempts = 100;
        int attempts = 0;
    
        do {
            spawnPos = GetRandomPosition();
            attempts++;
        } while (!IsValidSpawn(spawnPos) && attempts < maxAttempts);
    
        if (attempts < maxAttempts) {
            if (item == snowBall) {
                float rand = Random.Range(0f, 100f);
                if (rand > 90f) {
                    Instantiate(tenSnowBalls, spawnPos, Quaternion.identity);
                    return;
                }
            }
            Instantiate(item, spawnPos, Quaternion.identity);
        }
    }

    private int CountSnowBalls() {
        return FindObjectsOfType<SnowBall>().Length;
    }
    
    private Vector3 GetRandomPosition() {
        return new Vector3(
            // size of the iceberg in game world is approximately (25 x 25)
            Random.Range(-25f, 25f),
            Random.Range(-25f, 25f),
            0
        );
    }
    
    private bool IsValidSpawn(Vector3 position) {
        float minPlayerDistance = 5f;
        
        // check if too close to player
        if (Vector3.Distance(position, player.transform.position) < minPlayerDistance) {
            return false;
        }
        
        // check if too close to restricted areas
        foreach (Vector3 restrictedArea in restrictedAreas) {
            float restrictedRadius = 2.5f;
            if (Vector3.Distance(position, restrictedArea) < restrictedRadius) {
                return false;
            }
        }
    
        return true;
    }
}