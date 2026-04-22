using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to change the size of the box collider 2d for border signs depending on the screen size.
public class BorderWarningScript : MonoBehaviour
{
    private BoxCollider2D bc2d;
    private RectTransform rt;
    
    void Start(){
        bc2d = GetComponent<BoxCollider2D>();
        rt = GetComponent<RectTransform>();

        // setting the size of the box collider 2d with some offsets.
        var rect = rt.rect;
        bc2d.size = new Vector2(rect.width * 0.95f, (rect.height * 0.875f));
        float offsetY = rect.height * 0.03f;
        bc2d.offset = new Vector2(0, -offsetY);
    }
}
