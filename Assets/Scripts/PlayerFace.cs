using System.Collections;
using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    // referencing scripts.
    public LevelManager levelManagerScript;

    // referencing game objects.
    public Transform player;
    private Animator animator;

    // joystick.
    public Joystick joystick;

    // face movement variables.
    private float moveSpeed = 5f;
    private Vector2 movement;


    private void Start(){
        animator = GetComponent<Animator>();
    }
    void Update(){

        if (levelManagerScript.gameOver == false){
            float finalHorizontal = Mathf.Clamp(joystick.Horizontal, -0.3f, 0.3f);
            float finalVertical = Mathf.Clamp(joystick.Vertical, -0.3f, 0.3f);

            Vector3 desiredPosition  = player.position  + new Vector3(finalHorizontal, finalVertical, 0);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, moveSpeed * Time.unscaledDeltaTime);
            transform.position = smoothedPosition;
        }
    }
    
    public void SadFace(){
        animator.SetBool("Sad", true);
        StartCoroutine(BackToNormalFace());
    }

    public void HappyFace(){
        animator.SetBool("Happy", true);
        StartCoroutine(BackToNormalFace());
    }
    
    public void MeltFace(){
        animator.SetBool("Melt", true);
    }

    public IEnumerator BackToNormalFace(){
        yield return new WaitForSeconds(1);
        animator.SetBool("Sad", false);
        animator.SetBool("Happy", false);
    }
} 