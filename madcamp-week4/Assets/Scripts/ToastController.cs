using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToastMsg : MonoBehaviour
{
    public TextMeshProUGUI toast;
    float fadeInOutTime = 0.3f;
    static ToastMsg instance = null;

    public static ToastMsg Instance
    {
        get
        {
            if (null == instance) instance = FindObjectOfType<ToastMsg>();
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance) instance = this;
    }

    private void Start()
    {
        showMessage("", 0);
    }

    public void showMessage(string msg, float durationTime)
    {
        StartCoroutine(showMessageCoroutine(msg, durationTime));
    }

    private IEnumerator showMessageCoroutine(string msg, float durationTime)
    {
        toast.text = msg;
        toast.enabled = true;

        yield return fadeInOut(toast, fadeInOutTime, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < durationTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return fadeInOut(toast, fadeInOutTime, false);

        toast.enabled = false;
    }

    private IEnumerator fadeInOut(TextMeshProUGUI target, float durationTime, bool inOut)
    {
        float start, end;
        if (inOut)
        {
            start = 0.0f;
            end = 1.0f;
        }
        else
        {
            start = 1.0f;
            end = 0f;
        }

        Color current = Color.clear; /* (0, 0, 0, 0) = 검은색 글자, 투명도 100% */
        float elapsedTime = 0.0f;

        while (elapsedTime < durationTime)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / durationTime);

            target.color = new Color(current.r, current.g, current.b, alpha);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}