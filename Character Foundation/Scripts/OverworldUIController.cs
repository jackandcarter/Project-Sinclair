using UnityEngine;
using System.Collections;

public class OverworldUIController : MonoBehaviour
{
    public CanvasGroup overworldUIPanel; // Assign the UI panel with a CanvasGroup
    public float fadeDuration = 0.5f; // Duration for fade in/out
    public TopDownCharacterController characterController; // Reference to the player controller

    private Coroutine currentFadeCoroutine; // Tracks the currently running fade coroutine
    private bool isFadingOut; // Tracks the current fading direction
    private bool isFadingIn; // Tracks the current fading direction

    private void Update()
    {
        if (characterController == null || overworldUIPanel == null) return;

        bool isMoving = characterController.IsMoving();

        // Handle fade transitions based on movement state
        if (isMoving && !isFadingOut)
        {
            // Start fading out if moving
            StartFadeOut();
        }
        else if (!isMoving && !isFadingIn)
        {
            // Start fading in if stopped
            StartFadeIn();
        }
    }

    private void StartFadeOut()
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        isFadingOut = true;
        isFadingIn = false;

        currentFadeCoroutine = StartCoroutine(FadeCanvasGroup(overworldUIPanel, overworldUIPanel.alpha, 0f));
    }

    private void StartFadeIn()
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        isFadingIn = true;
        isFadingOut = false;

        currentFadeCoroutine = StartCoroutine(FadeCanvasGroup(overworldUIPanel, overworldUIPanel.alpha, 1f));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float targetAlpha)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        // Ensure final alpha value
        canvasGroup.alpha = targetAlpha;

        // Reset fading states
        isFadingOut = targetAlpha == 0f ? false : isFadingOut;
        isFadingIn = targetAlpha == 1f ? false : isFadingIn;

        currentFadeCoroutine = null;
    }
}
