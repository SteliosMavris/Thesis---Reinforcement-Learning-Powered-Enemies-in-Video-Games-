using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float orbitDistance;
    [SerializeField] private float mouseSensitivity;
    public float smoothTime;
    private float yaw, pitch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Keep orbit distance in the 1-20 range
        if(orbitDistance > 20)
        {
            orbitDistance = 20;
        }
        else if(orbitDistance < 1)
        {
            orbitDistance = 1;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -10, 80);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 position = target.position + Vector3.up * 1.5f - rotation * Vector3.forward * orbitDistance;
        transform.position = position;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}