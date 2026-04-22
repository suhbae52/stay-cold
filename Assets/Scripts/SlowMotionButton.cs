using UnityEngine;
using UnityEngine.EventSystems;

public class SlowMotionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // referencing script
    public bool buttonPressed;

    public void OnPointerDown(PointerEventData eventData){
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData evenData){
        buttonPressed = false;
    }

    
    // -------------------------------------------!Debug----------------------------------------------------------------
    private void FixedUpdate() {
        if (Input.GetKey("space")){
            buttonPressed = true; 
        }
        else{
            buttonPressed = false;
        }
    }
    // -------------------------------------------Debug!----------------------------------------------------------------
}
