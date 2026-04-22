using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingProjectile : MonoBehaviour
{
    // referencing scripts.
    private PlayerScript playerScript;
    private LevelManager levelManagerScript;

    // referencing game objects.
    private GameObject player;
    public AudioSource idleSound;
    public AudioSource beeping;

    // referencing self components
    private Rigidbody2D rb;

    // homing missile stats.
    private int damage = 50;
    private float speed = 10f;
    private float rotateSpeed = 3000f;
    private float homingDistance = 10f;

    // homing missile indicator.
    public GameObject indicator;
    private bool seen = false;
    private Renderer rd;
    
    // layer mask filter
    public ContactFilter2D filter;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        levelManagerScript = FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
        rb = GetComponent<Rigidbody2D>();
        rd = GetComponent<Renderer>();
        
        AudioManager.instance.RegisterAudioSource(idleSound);
        AudioManager.instance.RegisterAudioSource(beeping);
    }

    void Update(){
        // homing missile indicator.
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
                if(indicator.activeSelf){
                    indicator.SetActive(false);
                }
            }
        }

        // distance between missile and player.
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // triggering homing when it gets close enough to the player.
        if(distanceToPlayer < homingDistance && !levelManagerScript.gameOver){
            beeping.volume = 0.2f;
            // getting the velocity for movement
            Vector3 difference = player.transform.position - transform.position;
            float distance = difference.magnitude;
            Vector2 direction = difference / distance;
            rb.velocity = direction * speed;

            // getting the angular velocity for rotation
            Vector2 directionOfTurn = (Vector2)player.transform.position - rb.position;
            directionOfTurn.Normalize();
            float rotateAmount = Vector3.Cross(directionOfTurn, transform.right).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
        }else{
            beeping.volume = 0f;
            rb.angularVelocity = 0;
        }
    }
        
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            FindObjectOfType<AudioManager>().Play("Homing Missile Explosion");
            playerScript.TakeDamage(damage);
            DestroyProjectile();
        }if(other.CompareTag("ProjectileBoundary")){
            DestroyProjectile();
        }
    }

    void DestroyProjectile(){
        AudioManager.instance.UnregisterAudioSource(idleSound);
        AudioManager.instance.UnregisterAudioSource(beeping);
        Destroy(gameObject);
    }
}