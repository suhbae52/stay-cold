using UnityEngine;

// used for changing the alpha value of the ghosting effect on slow motion.
public class AlphaChange : MonoBehaviour{
    
    private SpriteRenderer sr;
    
    void Start(){
        sr = GetComponent<SpriteRenderer>();
    }

    void Update(){
        Color c = sr.color;
        c.a -= Time.unscaledDeltaTime;
        sr.color = c;
    }
}
