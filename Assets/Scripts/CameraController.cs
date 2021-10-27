using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = .125f;

    public Transform target;

    private Vector3 velocity = Vector3.zero;

    public Vector3 offset;

    public Vector3 cameraBoundsUpper;                //point where camera stops moving with player
    public Vector3 cameraBoundsLower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = PositionCameraInBounds(new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z));
        
        if(newPosition != transform.position)
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, speed);
    }

    public void SetCameraSceneDefaults(Vector3 newCameraBoundUpper, Vector3 newCameraBoundLower, float newCameraSpeed = .125f)
    {
        cameraBoundsUpper = newCameraBoundUpper;
        cameraBoundsLower = newCameraBoundLower;
        speed = newCameraSpeed;
    }

    private Vector3 PositionCameraInBounds(Vector3 newPosition)
    {
        Vector3 inBoundsPosition = newPosition;

        if (newPosition.x < cameraBoundsLower.x)
            inBoundsPosition.x = cameraBoundsLower.x;
        else if (newPosition.x > cameraBoundsUpper.x)
            inBoundsPosition.x = cameraBoundsUpper.x;
        if (newPosition.y < cameraBoundsLower.y)
            inBoundsPosition.y = cameraBoundsLower.y;
        else if (newPosition.y > cameraBoundsUpper.y)
            inBoundsPosition.y = cameraBoundsUpper.y;


        return inBoundsPosition;
    }
}
