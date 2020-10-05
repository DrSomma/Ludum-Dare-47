using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CanvasGroup grp;
    public float fadeSpeed = 2f;
    public float fadeOutTime = 2f;

    public void Init(int money)
    {
        text.text = (money > 0 ? "+" : "") + money + "$";
        text.color = (money > 0 ? Color.green : Color.red);
        StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        for (float t = 0.01f; t < fadeOutTime;)
        {
            transform.Translate(Vector3.up*0.3f);
            t += Time.deltaTime;
            t = Mathf.Min(t, fadeOutTime);
            canvasGroup.alpha = Mathf.Lerp(1, 0, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
    }
}
