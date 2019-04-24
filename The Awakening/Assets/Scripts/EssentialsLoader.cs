using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Load UI
        if (UIFade.instance == null)
        {
            Instantiate(UIScreen);
        }

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
