using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour{
    // referencing scripts
    private PlayerScript playerScript;
    
    // referencing game objects
    public GameObject ghostPrefab;
    
    // ghost objects and stats
    private SpriteRenderer sr;
    public Color color;
    public Material material = null;
    
    public float delay = 0.025f;
    private float delta;
    public float destroyTime = 0.3f;

    void Start(){
        playerScript = GetComponent<PlayerScript>();
    }

    void Update(){
        if (delta > 0){
            delta -= Time.deltaTime;
        }
        else{
            delta = delay;
            CreateGhost();
        }
    }

    void CreateGhost(){
        GameObject ghostObj = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostObj.transform.localScale = playerScript.transform.localScale;
        Destroy(ghostObj, destroyTime);

        sr = ghostObj.GetComponent<SpriteRenderer>();
        sr.sprite = playerScript.sr.sprite;
        sr.color = color;
        if (material != null){
            sr.material = material;
        }
    }
}
