using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [Range(0, 2)]
    public float fadeSpeed = 1;

    public UnityEvent OnFadeIn;
    public UnityEvent OnFadeOut;

    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        float i = canvasGroup.alpha;
        // loop over 1 second
        for (i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            canvasGroup.alpha = i;
            yield return null;
        }
        OnFadeIn?.Invoke();
    }

    IEnumerator FadeOutRoutine()
    {
        float i = canvasGroup.alpha;
        // loop over 1 second backwards
        for (i = 1; i >= 0; i -= Time.deltaTime * fadeSpeed)
        {
            // set color with i as alpha
            canvasGroup.alpha = i;
            yield return null;
        }
        OnFadeOut?.Invoke();
    }
}
