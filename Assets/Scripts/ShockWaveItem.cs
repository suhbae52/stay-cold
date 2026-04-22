using System.Collections;
using UnityEngine;

public class ShockWaveItem : MonoBehaviour
{
    // referencing game objects.
    private GameObject player;
    public GameObject shockWave;
    private GameObject levelManager;
    private LevelManager levelManagerScript;
    private AudioSource audioSource;

    // shock wave item indicator
    public GameObject indicator;
    private Renderer rd;
    
    // layer mask filter
    public ContactFilter2D filter;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        levelManager = GameObject.Find("LevelManager");
        levelManagerScript = levelManager.GetComponent<LevelManager>();
        audioSource = GetComponent<AudioSource>();
        AudioManager.instance.RegisterAudioSource(audioSource);
        rd = GetComponent<Renderer>();
    }

    void Update(){
        // shock wave item indicator
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
            if(indicator.activeSelf){
                indicator.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            FindObjectOfType<AudioManager>().Play("Item Pickup Sound");
            levelManagerScript.AddScore(500f);
            Instantiate(shockWave, transform.position, Quaternion.identity);
            AudioManager.instance.UnregisterAudioSource(audioSource);
            Destroy(gameObject);
        }
    }
}