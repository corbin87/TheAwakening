using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    // Awake is called before Start() and before the first frame update
    void Awake()
    {
        // If player is not loaded into the scene, load player
        if (PlayerController.instance == null)
        {
            Instantiate(player);
            player.transform.position = this.gameObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
