using System.Collections;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class MoneyText : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public CanvasGroup canvasGroup;
        public float fadeSpeed = 0.3f;
        public float fadeOutTime = 2f;

        public void Init(int money)
        {
            text.text = (money > 0 ? "+" : "") + money + "$";
            text.color = (money > 0 ? Color.green : Color.red);
            canvasGroup = GetComponent<CanvasGroup>();
            StartCoroutine(FadeAway());
        }

        private IEnumerator FadeAway()
        {
            if (canvasGroup is null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            for (float t = 0.01f; t < fadeOutTime;)
            {
                transform.Translate(Vector3.up*fadeSpeed);
                t += Time.deltaTime;
                t = Mathf.Min(a: t, b: fadeOutTime);
                canvasGroup.alpha = Mathf.Lerp(a: 1, b: 0, t: Mathf.Min(a: 1, b: t / fadeOutTime));
                yield return null;
            }
        }
    }
}
