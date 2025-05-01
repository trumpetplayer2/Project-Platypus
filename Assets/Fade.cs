using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Color hidden = Color.clear;
    public Color shown = Color.black;
    public Image FadeImage;
    public bool fading = true;
    public bool fadeDirection = false;
    public float fadeTime = 1f;
    float timer = 0f;
    public static Fade instance;

    // Start is called before the first frame update
    void Start()
    {
        FadeImage.color = shown;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fading) return;
        if (timer > fadeTime)
        {
            timer = 0f;
            fading = false;
            FadeImage.color = (fadeDirection) ? shown : hidden;
            return;
        }
        if (fadeDirection)
        {
            Color temp = FadeImage.color;
            temp.a = timer / fadeTime;
            FadeImage.color = temp;
        }
        else
        {
            Color temp = FadeImage.color;
            temp.a = 1 - (timer / fadeTime);
            FadeImage.color = temp;
        }
        timer += Time.deltaTime;
    }
}
