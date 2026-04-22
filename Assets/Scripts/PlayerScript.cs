using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    // referencing scripts.
    public LevelManager levelManagerScript;
    public SlowMotionButton slowMotionButtonScript;
    public PlayerFace playerFaceScript;
    public GhostEffect ghostEffectScript;
    public CameraScript cameraScript;
    [SerializeField] private FullScreenEffectController fullScreenEffectControllerScript;

    // referencing game objects.
    private Animator animator;
    public ParticleSystem dust;
    private Rigidbody2D rb2d;
    public SpriteRenderer sr;
    private PolygonCollider2D pc2d;
    
    // number pop up
    public GameObject damageIndicator;
    public GameObject healthIndicator;
    public GameObject energyIndicator;

    // energy bar.
    private float maxEnergy;
    private float energy;
    public Slider energySlider;
    private float initialEnergyUsage = 3f;
    private bool initialEnergyBool;

    // health bar.
    private float maxHealth;
    private float health;
    public Slider healthSlider;

    // joystick.
    public VariableJoystick joystick;
    private JoystickType joyStickType;

    // etc stats
    private float playerSize = 10f;
    private float moveSpeed;

    // Octagon vertices (adjust this based on your map's shape)
    public Vector2[] octagonVertices;

    void Start() {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        pc2d = GetComponent<PolygonCollider2D>();
        
        //initialize dataManager if it doesn't exist (probably debug feature)
        DataManager.Initialize();
        
        // setting the player values.
        maxHealth = DataManager.instance.health;
        maxEnergy = DataManager.instance.energy;
        moveSpeed = DataManager.instance.speed;
        
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        energy = maxEnergy;
        energySlider.maxValue = maxEnergy;
        energySlider.value = maxEnergy;

        SwitchJoyStickType(PlayerPrefs.GetInt("Joystick Type", 1));

        // Define the octagon vertices here (you can adjust this based on your map shape)
        octagonVertices = new Vector2[] {
            new Vector2(34f, 18f),   // Right
            new Vector2(23f, 29f),    // Top-right
            new Vector2(-25f, 29f),   // Top
            new Vector2(-36f, 18f),   // Top-left
            new Vector2(-36f, -18f),  // Left
            new Vector2(-25f, -29f),  // Bottom-left
            new Vector2(23f, -29f),  // Bottom
            new Vector2(34f, -18f),   // Bottom-right
        };
    }

    public void Update() {
        if (!levelManagerScript.gameOver) {
            
            // slow motion functionality
            if (energy > 0 && slowMotionButtonScript.buttonPressed) {
                levelManagerScript.slowMo = true;
                ghostEffectScript.enabled = true;
                cameraScript.zoomBool = true;
                fullScreenEffectControllerScript.SMEBool = true;
                if (!initialEnergyBool) {
                    energy -= initialEnergyUsage;
                    FindObjectOfType<AudioManager>().Play("Slow Motion");
                    initialEnergyBool = true;
                }
            
                energy -= Mathf.Lerp(0, 100, Time.unscaledDeltaTime * 0.1f);
                if (energy <= 0) {
                    energy = 0;
                }
            
                energySlider.value = energy;
            }
            else {
                levelManagerScript.slowMo = false;
                ghostEffectScript.enabled = false;
                cameraScript.zoomBool = false;
                fullScreenEffectControllerScript.SMEBool = false;
                initialEnergyBool = false;
            }
        }
        else {
            // turn off all effects if game ends while slow motion.
            levelManagerScript.slowMo = false;
            ghostEffectScript.enabled = false;
            cameraScript.zoomBool = false;
            fullScreenEffectControllerScript.SMEBool = false;
        }
    }

    void FixedUpdate() {
        if (!levelManagerScript.gameOver) {
            Vector2 movement = new Vector2(joystick.Horizontal, joystick.Vertical);
            animator.SetFloat("Speed", Mathf.Abs(movement.x));
            CreateDust();
            // Simply move the player, limiting the movement to within bounds
            MovePlayer(movement);
             
            if (movement.x > 0f) {
                transform.localScale = new Vector2(-playerSize, playerSize);
            }
            else if (movement.x < 0f) {
                transform.localScale = new Vector2(playerSize, playerSize);
            }
        }
        
    }

    public void MovePlayer(Vector2 direction) {
        // Calculate the desired position based on movement
        Vector2 moveVector = Time.unscaledDeltaTime * moveSpeed * direction;
        Vector2 desiredPosition = rb2d.position + moveVector;

        // Check if the desired position goes out of bounds and clamp it if necessary
        Vector2 clampedPosition = ClampPositionToOctagon(desiredPosition);
        
        // Apply the movement (clamped or not)
        rb2d.MovePosition(clampedPosition);
        //rb2d.MovePosition(Vector2.Lerp(rb2d.position, clampedPosition, 0.1f));
    }

    // Function to check if a position is inside the octagon
    bool IsInsideOctagon(Vector2 position) {
        int i, j;
        bool inside = false;

        for (i = 0, j = octagonVertices.Length - 1; i < octagonVertices.Length; j = i++) {
            if ((octagonVertices[i].y > position.y) != (octagonVertices[j].y > position.y) &&
                (position.x < (octagonVertices[j].x - octagonVertices[i].x) * (position.y - octagonVertices[i].y) / (octagonVertices[j].y - octagonVertices[i].y) + octagonVertices[i].x)) {
                inside = !inside;
            }
        }

        return inside;
    }

    // Function to clamp the position within the octagon boundary
    Vector2 ClampPositionToOctagon(Vector2 position) {
        if (IsInsideOctagon(position)) {
            return position;  // The position is within bounds
        } else {
            // Find the closest point on the octagon's boundary
            Vector2 closestPoint = FindClosestPointOnOctagon(position);
            return closestPoint;  // Return the closest point on the boundary
        }
    }

    // Function to find the closest point on the octagon boundary to the player
    Vector2 FindClosestPointOnOctagon(Vector2 position) {
        Vector2 closestPoint = position;
        float minDistance = float.MaxValue;

        // Loop through each edge of the octagon
        for (int i = 0; i < octagonVertices.Length; i++) {
            Vector2 vertexA = octagonVertices[i];
            Vector2 vertexB = octagonVertices[(i + 1) % octagonVertices.Length];

            // Get the closest point on the edge of the octagon
            Vector2 pointOnEdge = GetClosestPointOnEdge(position, vertexA, vertexB);
            float distance = Vector2.Distance(position, pointOnEdge);

            // Update the closest point if necessary
            if (distance < minDistance) {
                minDistance = distance;
                closestPoint = pointOnEdge;
            }
        }

        return closestPoint;
    }

    // Function to get the closest point on an edge
    Vector2 GetClosestPointOnEdge(Vector2 position, Vector2 vertexA, Vector2 vertexB) {
        Vector2 edgeDirection = vertexB - vertexA;
        Vector2 toPosition = position - vertexA;

        // Project the position onto the edge
        float projection = Vector2.Dot(toPosition, edgeDirection.normalized);
        projection = Mathf.Clamp(projection, 0, edgeDirection.magnitude);

        return vertexA + edgeDirection.normalized * projection;
    }




    // Other methods (like taking damage, healing, etc.) remain unchanged
     public void TakeDamage(int amount) {
         StartCoroutine(cameraScript.CameraShakeActive(.25f, 0.2f));
         fullScreenEffectControllerScript.Hurt();
         FindObjectOfType<AudioManager>().Play("Damaged Sound");
         health -= amount;
         healthSlider.value = health;
         InstantiateNumberPopUp(amount, damageIndicator);
         
         if (health <= 0) {
             health = 0;
             StartCoroutine(levelManagerScript.Die());
             animator.SetBool("Alive", false);
             playerFaceScript.MeltFace();
             pc2d.enabled = !pc2d.enabled;
         }

         playerFaceScript.SadFace();
     }

     public void HealthHeal(int amount) {
         health += amount;
         healthSlider.value = health;
         InstantiateNumberPopUp(amount, healthIndicator);

         if (health > maxHealth) {
             health = maxHealth;
         }

         playerFaceScript.HappyFace();
     }

     public void EnergyHeal(float amount) {
         energy += amount;
         energySlider.value = energy;
         InstantiateNumberPopUp(amount, energyIndicator);

         if (energy > maxEnergy) {
             energy = maxEnergy;
         }

         playerFaceScript.HappyFace();
     }

     void InstantiateNumberPopUp(float amount, GameObject indicator) {
         NumberIndicator numberPopUp = Instantiate(indicator, transform.position, Quaternion.identity).GetComponent<NumberIndicator>();
         numberPopUp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
         numberPopUp.SetText(amount);
     }

    public void SwitchJoyStickType(int type) {
        if (type == 1) {
            joystick.SetMode(JoystickType.Fixed);
            PlayerPrefs.SetInt("Joystick Type", 1);
        }
        else {
            joystick.SetMode(JoystickType.Floating);
            PlayerPrefs.SetInt("Joystick Type", 2);
        }
    }

    void CreateDust() {
        dust.Play();
    }
}