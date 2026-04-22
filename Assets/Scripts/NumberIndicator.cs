using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberIndicator : MonoBehaviour {
    //public Transform playerTransform;
    
    public TextMeshProUGUI text;
    public float lifeTime = 1.5f;
    public float distance = 1.5f;
    
    private Vector3 startPos;
    private Vector3 endPos;
    private float timer;
    
    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(0f, distance, 0f);
    }
    void Update() {
        timer += Time.deltaTime;

        float fraction = lifeTime / 2f;
        
        if (timer > lifeTime) Destroy(gameObject);
        else if (timer > fraction)
            text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifeTime - fraction));
        transform.position = Vector3.Lerp(startPos, endPos, Mathf.Sin(timer / lifeTime));
    }

    public void SetText(float damage) {
        text.text = damage.ToString("0");
    }
}
