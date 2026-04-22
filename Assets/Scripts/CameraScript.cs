using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera cam;

    private float zoomMin = 14f;
    private float zoomMax = 15f;
    private float zoomScale = 2f;
    private float zoomSignum = 1f;
    private float zoom;
    public bool zoomBool = false;

    private void Start(){
        zoom = zoomMax;
    }
    
    private void Update(){
        zoomSignum = zoomBool ? -1 : Mathf.Abs(zoomSignum);
        zoom += (zoomScale * zoomSignum * Time.unscaledDeltaTime);
        if (zoom < zoomMin) zoom = zoomMin;
        if (zoom > zoomMax) zoom = zoomMax;
  
        cam.orthographicSize = zoom;
    }
    public IEnumerator CameraShakeActive(float duration, float magnitude){
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration){
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
