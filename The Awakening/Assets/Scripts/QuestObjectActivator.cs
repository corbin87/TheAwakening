using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    // Quest object tracking
    public GameObject objectToActivate;
    public string questToCheck;
    public bool activeIfComplete;
    private bool initialCheckDone;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialCheckDone)
        {
            initialCheckDone = true;

            CheckCompletion();
        }
    }

    // Activate and show quest object on complete if boolean is set properly
    public void CheckCompletion()
    {
        if (QuestManager.instance.CheckIfComplete(questToCheck))
        {
            Debug.Log("Activating quest object");
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
