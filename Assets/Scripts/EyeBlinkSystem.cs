using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EyeBlinkSystem : MonoBehaviour
{
    [Header("Eye Setup")]
    [SerializeField] private Image closedEyeImage; // Reference to the Image component for closed eye
    
    [Header("Blink Settings")]
    [SerializeField] private float minBlinkInterval = 2f; // Minimum time between blinks
    [SerializeField] private float maxBlinkInterval = 5f; // Maximum time between blinks
    [SerializeField] private float blinkSpeed = 10f; // Speed of the blink animation
    [SerializeField] private bool autoBlink = true; // Toggle for automatic blinking

    private void Start()
    {
        if (closedEyeImage == null)
        {
            Debug.LogError("Closed Eye Image reference is missing!");
            enabled = false;
            return;
        }

        // Ensure closed eye starts fully transparent
        SetEyeTransparency(0);

        if (autoBlink)
        {
            StartCoroutine(BlinkRoutine());
        }
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
            // Wait for random interval before next blink
            float waitTime = Random.Range(minBlinkInterval, maxBlinkInterval);
            yield return new WaitForSeconds(waitTime);

            // Perform blink
            yield return StartCoroutine(PerformSmoothBlink());
        }
    }

    private IEnumerator PerformSmoothBlink()
    {
        // Smoothly close eye
        float currentAlpha = 0f;
        while (currentAlpha < 1f)
        {
            currentAlpha += Time.deltaTime * blinkSpeed;
            SetEyeTransparency(Mathf.Clamp01(currentAlpha));
            yield return null;
        }

        // Small pause when fully closed
        yield return new WaitForSeconds(0.1f);

        // Smoothly open eye
        while (currentAlpha > 0f)
        {
            currentAlpha -= Time.deltaTime * blinkSpeed;
            SetEyeTransparency(Mathf.Clamp01(currentAlpha));
            yield return null;
        }
    }

    // Public method to trigger a manual blink
    public void TriggerBlink()
    {
        StartCoroutine(PerformSmoothBlink());
    }

    // Public method to toggle automatic blinking
    public void ToggleAutoBlink(bool enable)
    {
        autoBlink = enable;
        if (autoBlink)
        {
            StartCoroutine(BlinkRoutine());
        }
    }
}
