using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = System.Numerics.Vector3;

public class FireBall : MonoBehaviour
{
    // referencing scripts.
    private GameObject player;
    private PlayerScript playerScript;

    // fireball trail.
    public ParticleSystem fireTrail;

    // fireball stats.
    private int damage = 25;

    // fireball indicator.
    public GameObject indicator;
    private bool seen = false;
    private Renderer rd;
    
    // audio source.
    private AudioSource audioSource;
    
    // layer mask filter
    public ContactFilter2D filter;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();

        rd = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        AudioManager.instance.RegisterAudioSource(audioSource);
        PlayFireTrail();
    }
    
    void Update(){
        if(!seen){
            if(!rd.isVisible){
                if(!indicator.activeSelf){
                    indicator.SetActive(true);
                }
                Vector2 direction = player.transform.position - transform.position;
                RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, filter.layerMask);
                if (ray.collider != null){
                    indicator.transform.position = ray.point;
                }
            }else{
                seen = true;
                if (indicator.activeSelf ){
                    indicator.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            playerScript.TakeDamage(damage);
            FindObjectOfType<AudioManager>().Play("Fireball Explosion");
            DestroyProjectile();
        }
        if(other.CompareTag("ProjectileBoundary")){
            DestroyProjectile();
        }
    }

    void PlayFireTrail(){
        fireTrail.Play();
    }
    void DestroyProjectile(){
        AudioManager.instance.UnregisterAudioSource(audioSource);
        Destroy(gameObject);
    }
}