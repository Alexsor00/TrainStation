using System.Collections;
using UnityEngine;

public class AvatarDetector : MonoBehaviour
{
    public static AvatarDetector instance;

    [Tooltip("Distance at which avatars are detected.")]
    public float distanceOfAvatarDetection;

    public GameObject currentlyInteractingAvatar;
    public GameObject currentlyObservedAvatar;

    private const float DISTANCE_BUFFER = 0.2f;
    private const float AVATAR_DETECTION_INTERVAL = 0.2f; // Changed interval from 0.1f to 0.2f
    private const float AVATAR_MEMORY_DURATION = 2.0f;
    private LayerMask personLayer;  // Layer for person 

    private float timer;
    private Vector3 smoothedDirection;
    private bool isInDetectionRange = false;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        personLayer = LayerMask.GetMask("Person");
    }

   

    public void GreetAvatar()
    {
        if(currentlyObservedAvatar)
        {
            currentlyObservedAvatar.GetComponent<Interaction>().greeted = true;
        }
    }

    

  

 public GameObject DetectAvatar()  // Note the return type change to GameObject
{
    float rayRadius = 0.5f; 
    Vector3 desiredDirection = transform.TransformDirection(Vector3.forward) * distanceOfAvatarDetection + transform.TransformDirection(Vector3.down);
    smoothedDirection = Vector3.Lerp(smoothedDirection, desiredDirection, Time.deltaTime * 5f);

    if (Physics.SphereCast(transform.position, rayRadius, smoothedDirection, out RaycastHit hit, distanceOfAvatarDetection, personLayer))
    {
        float distanceToAvatar = Vector3.Distance(transform.position, hit.collider.transform.position);
        HandleAvatarInRange(distanceToAvatar, hit.collider.gameObject);
        return hit.collider.gameObject;  // Return the detected avatar
    }
    else
    {
        currentlyObservedAvatar = null;
        return null;  // No avatar was detected
    }
}

private void HandleAvatarInRange(float distance, GameObject avatar)
{
    if (!isInDetectionRange && distance <= (distanceOfAvatarDetection - DISTANCE_BUFFER))
    {
        isInDetectionRange = true;
    }
    else if (isInDetectionRange && distance >= (distanceOfAvatarDetection + DISTANCE_BUFFER))
    {
        isInDetectionRange = false;
    }

    if (isInDetectionRange)
    {
        timer = AVATAR_MEMORY_DURATION;
        currentlyObservedAvatar = avatar;
    }
}

}
