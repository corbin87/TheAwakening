using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaTransition : MonoBehaviour
{
    [SerializeField]
    private string transitionName;
    [SerializeField]
    private string transitioningTo;


    // Start is called before the first frame update
    void Start()
    {
        // On scene load, set player position to transition position
        // IF player's destination matches transition name
        // BE CAREFUL TO FOLLOW NAMING CONVENTIONS TO AVOID UNEXPECTED BEHAVIOR
        if (PlayerController.instance != null)
        {
            if (transitionName == PlayerController.instance.areaTransitionedTo)
            {
                PlayerController.instance.transform.position = transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only transition to new scene if player is entering transition area for first time
        if (PlayerController.instance.inTransitionArea == false)
        {
            Debug.Log("Transitioning to " + transitioningTo);
            // Switch scenes
            if (other.tag == "Player")
            {
                // Get scene name from transitioningTo variable through naming convention
                PlayerController thePlayer = other.gameObject.GetComponent<PlayerController>();
                thePlayer.inTransitionArea = true;
                thePlayer.transitionDone = false;
                string sceneName;
                int index = transitioningTo.IndexOf("-");
                sceneName = transitioningTo.Substring(0, index);
                thePlayer.areaTransitionedTo = transitioningTo;

                // Start transition
                UIFade.instance.FadeIn();
                StartCoroutine(loadSceneAfterFade(sceneName));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PlayerController.instance.inTransitionArea == true)
        {
            PlayerController.instance.inTransitionArea = false;
        }
    }

    // Coroutine to load scene after fade finishes
    private IEnumerator loadSceneAfterFade(string theScene)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(theScene);
    }
}
