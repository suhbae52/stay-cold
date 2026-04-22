using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ShockWave : MonoBehaviour{
    private AudioSource audioSource;
    public LevelManager levelManagerScript;
    
    private Vector3 targetScale = new Vector3(100f,100f,100f);
    private Vector3 startScale;
    private float t = 0;
    void Start(){
        audioSource = GetComponent<AudioSource>();
        AudioManager.instance.RegisterAudioSource(audioSource);
        
        levelManagerScript = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        
        StartCoroutine(DeSpawn());
        startScale = transform.localScale;
        t = 0;
    }

    void Update(){
        t += Time.deltaTime;
        
        Vector3 newScale = Vector3.Lerp(startScale, targetScale, t);
        transform.localScale = newScale;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Projectile")){
            if (other.gameObject.transform.parent){
                Destroy(other.gameObject.transform.parent.gameObject);
            }
            levelManagerScript.AddScore(50);
            Destroy(other.gameObject);
        }
    }

    IEnumerator DeSpawn(){
        yield return new WaitForSeconds(1);
        AudioManager.instance.UnregisterAudioSource(audioSource);
        Destroy(gameObject);
    }
}