using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    public Tilemap map;

    // Stops camera at tilemap border
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;
    // Stops camera before background of scene is visible
    private float halfHeight;
    private float halfWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set in update to avoid unallocated bug
        //target = PlayerController.instance.transform;

        // Do not allow camera to show background
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        // Limit the camera to the current scene tilemap size
        bottomLeftLimit = map.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = map.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

        // Set player bounds to map size
        PlayerController.instance.SetBounds(map.localBounds.min, map.localBounds.max);
    }

    // LateUpdate is called once per frame after all Updates
    void LateUpdate()
    {
        // Once player is loaded into scene, tie camera vector to player vector
        if (target == null)
        {
            Debug.Log("Setting camera target");
            target = PlayerController.instance.transform;
        }

        // Move map with the player character
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Keep camera within bounds of scene
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), 
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), 
            transform.position.z);
    }
}
