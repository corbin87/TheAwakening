using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;
    public Image fadeScreen;

    public bool shouldFadeIn;
    public bool shouldFadeOut;
    public float fadeSpeed;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        fadeSpeed = 1.0f;
    }

        // Update is called once per frame
        void Update()
    {
        if (shouldFadeIn)
        {
            // Increase alpha value to start scene transition
            fadeScreen.color = new Color(
                fadeScreen.color.r,
                fadeScreen.color.g,
                fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1.0f, fadeSpeed * Time.deltaTime));

            // Stop fade when done
            if (fadeScreen.color.a == 1.0f)
            {
                shouldFadeIn = false;
            }
        }

        if (shouldFadeOut)
        {
            // Decrease alpha value to finish scene transition
            fadeScreen.color = new Color(
                fadeScreen.color.r,
                fadeScreen.color.g,
                fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0.0f, fadeSpeed * Time.deltaTime));

            // Stop fade when done
            if (fadeScreen.color.a == 0.0f)
            {
                shouldFadeOut = false;
            }
        }
    }

    public void FadeIn()
    {
        shouldFadeIn = true;
        shouldFadeOut = false;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1);
        shouldFadeOut = true;
        shouldFadeIn = false;
        StartCoroutine(FadeFinished());
    }

    private IEnumerator FadeFinished()
    {
        yield return new WaitForSeconds(1);
        PlayerController.instance.transitionDone = true;
    }
}
