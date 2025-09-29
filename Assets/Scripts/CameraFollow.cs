using UnityEngine;

 public class CameraFollow : MonoBehaviour
{/*
    private Transform target;
    private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void LateUpdate()
    {
        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
        var targetPos = new Vector3(target.position.x, target.position.y, -10);
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        
    }
}
*/
public Transform target;
public float smoothing;

    void FixedUpdate()
    {
    Vector3 targetPosition = new Vector3
    (target.position.x, target.position.y, 
    transform.position.z); 

    transform.position = Vector3.Lerp
    (transform.position, 
    targetPosition, smoothing*Time.deltaTime);
    }
}