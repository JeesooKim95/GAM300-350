/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 09/26/2021
    Desc    : Fade in/out effect
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType
{
    FadeIn, FadeOut, FadeInAndOut
}

public class Fade : MonoBehaviour
{
    public RectTransform canvas = null;
    public Color fadeColor = Color.black;
    private bool isEffectDone = true;
    private Image image = null;
    private bool isDark = false;

    public bool IsDark()
    {
        return isDark;
    }
    
    public bool IsEffectDone()
    {
        return isEffectDone;
    }

    private void CreateImage()
    {
        if(image == null)
        {
            image = new GameObject("Fade Image", typeof(Image)).GetComponent<Image>();
            image.color = fadeColor;
            RectTransform rect = image.GetComponent<RectTransform>();
            rect.SetParent(canvas);
            rect.localPosition = new Vector3(1, 1, 1);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localScale = new Vector3(10, 10, 1);
        }
    }
    private void DeleteImage()
    {
        if(image)
        {
            Destroy(image.gameObject);
            image = null;
        }
    }

    private IEnumerator FadeIn(float time)
    {
        float step = 1.0f / time;
        for (float elapsed = 0; elapsed <= time; elapsed += Time.deltaTime)
        {
            float w = step * elapsed;
            image.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, w);
            isDark = (w > 0.8f) ? true : false;
            yield return null;
        }
        image.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        isEffectDone = true;
    }
    
    private IEnumerator FadeOut(float time)
    {
        float step = 1.0f / time;
        for (float elapsed = 0; elapsed <= time; elapsed += Time.deltaTime)
        {
            float w = 1 - step * elapsed;
            image.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, w);
            isDark = (w > 0.8f) ? true : false;
            yield return null;
        }
        DeleteImage();
        isEffectDone = true;
    }

    private IEnumerator FadeInAndOut(float time)
    {
        float step = 2.0f / time;
        for (float elapsed = 0; elapsed <= time; elapsed += Time.deltaTime)
        {
            float w = step * elapsed;
            w = (w > 1) ? w : 2.0f - w;
            image.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, w);
            isDark = (w > 0.8f) ? true : false;
            yield return null;
        }
        DeleteImage();
        isEffectDone = true;
    }

    public void StartAction(FadeType type, float period)
    {
        isEffectDone = false;
        CreateImage();
        switch (type)
        {
            case FadeType.FadeIn:
                StartCoroutine(FadeIn(period));
                break;
            case FadeType.FadeOut:
                StartCoroutine(FadeOut(period));
                break;
            case FadeType.FadeInAndOut:
                StartCoroutine(FadeInAndOut(period));
                break;
        }
    }
}
