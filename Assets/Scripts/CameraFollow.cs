using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    private float camSpeed = 7.5f;

    void Update(){
        Vector3 desiredPosition = player.position + new Vector3(0, 0, -10);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, camSpeed * Time.deltaTime);
    }
}
