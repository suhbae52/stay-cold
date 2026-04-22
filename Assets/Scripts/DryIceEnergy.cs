using System.Collections;
using UnityEngine;

public class DryIceEnergy : MonoBehaviour
{
    // referencing scripts
    private PlayerScript playerScript;
    private LevelManager levelManagerScript;
    
    // referencing game objects.
    private GameObject player;
    private GameObject levelManager;
    private AudioSource audioSource;
    
    // dry ice indicator
    public GameObject indicator;
    private Renderer rd;
    private SpriteRenderer sr;

    // dry ice stats
    private float energyHealAmount;
    
    // layer mask filter
    public ContactFilter2D filter;
    
    // etc stats
    public float lifeTime = 30f;
    public float blinkDuration = 5f;
    public float blinkInterval = 0.5f;

    public bool isBlinking;
    
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        levelManager = GameObject.Find("LevelManager");
        levelManagerScript = levelManager.GetComponent<LevelManager>();
        audioSource = GetComponent<AudioSource>();
        AudioManager.instance.RegisterAudioSource(audioSource);
        rd = GetComponent<Renderer>();
        sr = GetComponent<SpriteRenderer>();

        energyHealAmount = DataManager.instance.energy;
        StartCoroutine(DestroySelf());
    }

    void Update(){
        // dry ice indicator
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
            playerScript.EnergyHeal(energyHealAmount);
            Destroy(gameObject);
        }
    }
    
    IEnumerator DestroySelf(){
        // wait til it's time to start blinking.
        yield return new WaitForSeconds(lifeTime - blinkDuration);
        
        // start blinking
        isBlinking = true;
        StartCoroutine(Blink());
        
        // wait remaining lifetime.
        yield return new WaitForSeconds(blinkDuration);
        AudioManager.instance.UnregisterAudioSource(audioSource);
        Destroy(gameObject);
    }

    IEnumerator Blink() {
        Color spriteColor = sr.color;
        while (isBlinking) {
            // Toggle alpha value.
            spriteColor.a = 0f;
            sr.color = spriteColor;
            yield return new WaitForSeconds(blinkInterval/2);
            spriteColor.a = 256f;
            sr.color = spriteColor;
            yield return new WaitForSeconds(blinkInterval/2);
        }
    }
}
