using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for handling movement between different base areas. Should be attached to any door in the base with the coordinates for where the door take the player, as well as the new camera bounds for the area
public class BaseTransporterController : MonoBehaviour
{
    [SerializeField] private Vector2 movementLocation;
    [SerializeField] private Vector3 newCameraBoundUpper;
    [SerializeField] private Vector3 newCameraBoundLower;
    //private Box

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MovePlayerToLocation()
    {
        WorldStateInfo.Instance.player.BaseTravelMovement(movementLocation);
        WorldStateInfo.Instance.mainCamera.SetCameraSceneDefaults(newCameraBoundUpper, newCameraBoundLower);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            MovePlayerToLocation();
        }
    }
}
