using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private Image fadeImage;
    public GameObject go;

    void Update()
    {
        fadeImage = go.gameObject.GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        for (int i = 0; i < 10; i++)
        {
            float f = i / 10.0f;
            Color c = fadeImage.color;
            c.a = f;
            fadeImage.color = c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator FadeOut()
    {
        for(int i = 10; i >= 0; i--) 
        {
            float f = i / 10.0f;
            Color c = fadeImage.color;
            c.a = f;
            fadeImage.color = c;
            yield return new WaitForSeconds(0.1f);
        }
        
    }

}
