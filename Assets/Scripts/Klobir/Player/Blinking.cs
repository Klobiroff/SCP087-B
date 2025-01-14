using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Klobir.Player
{
    public class Blinking : MonoBehaviour
    {
        [Header("Eye Setup")]
        [SerializeField] private Image closedEyeImage;

        [Header("Blink Settings")]
        [SerializeField] private float minBlinkInterval = 2f;
        [SerializeField] private float maxBlinkInterval = 5f;
        [SerializeField] private float blinkSpeed = 10f;
        [SerializeField] private bool autoBlink = true;

        private void Start()
        {
            if (closedEyeImage == null)
            {
                Debug.LogError("Closed Eye Image reference is missing!");
                enabled = false;
                return;
            }

            SetEyeTransparency(0);

            if (autoBlink)
                StartCoroutine(BlinkRoutine());
        }

        private void SetEyeTransparency(float alpha)
        {
            Color eyeColor = closedEyeImage.color;
            eyeColor.a = alpha;
            closedEyeImage.color = eyeColor;
        }

        private IEnumerator BlinkRoutine()
        {
            while (autoBlink)
            {
                float waitTime = Random.Range(minBlinkInterval, maxBlinkInterval);
                yield return new WaitForSeconds(waitTime);

                yield return StartCoroutine(PerformSmoothBlink());
            }
        }

        private IEnumerator PerformSmoothBlink()
        {
            float currentAlpha = 0f;
            while (currentAlpha < 1f)
            {
                currentAlpha += Time.deltaTime * blinkSpeed;
                SetEyeTransparency(Mathf.Clamp01(currentAlpha));
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);

            while (currentAlpha > 0f)
            {
                currentAlpha -= Time.deltaTime * blinkSpeed;
                SetEyeTransparency(Mathf.Clamp01(currentAlpha));
                yield return null;
            }
        }

        public void TriggerBlink()
        {
            StartCoroutine(PerformSmoothBlink());
        }

        public void ToggleAutoBlink(bool enable)
        {
            autoBlink = enable;
            if (autoBlink)
                StartCoroutine(BlinkRoutine());
        }
    }
}
