using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles simple cutscene events such as camera movement and playing animations.
/// </summary>
public class CutsceneManager : MonoBehaviour
{
    public Camera cutsceneCamera;

    /// <summary>
    /// Plays a list of cutscene events sequentially.
    /// </summary>
    public IEnumerator PlayCutscene(List<CutsceneEvent> events)
    {
        foreach (CutsceneEvent e in events)
        {
            switch (e.type)
            {
                case CutsceneEventType.MoveCamera:
                    if (cutsceneCamera != null && e.target != null)
                    {
                        yield return StartCoroutine(MoveCamera(e.target, e.duration));
                    }
                    break;
                case CutsceneEventType.PlayAnimation:
                    if (e.animator != null)
                    {
                        e.animator.Play(e.animationName);
                    }
                    break;
                case CutsceneEventType.Wait:
                    yield return new WaitForSeconds(e.duration);
                    break;
            }
        }
    }

    private IEnumerator MoveCamera(Transform target, float duration)
    {
        Vector3 startPos = cutsceneCamera.transform.position;
        Quaternion startRot = cutsceneCamera.transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            cutsceneCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            cutsceneCamera.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        cutsceneCamera.transform.position = endPos;
        cutsceneCamera.transform.rotation = endRot;
    }
}

/// <summary>
/// Represents a single cutscene event.
/// </summary>
[System.Serializable]
public class CutsceneEvent
{
    public CutsceneEventType type;
    public Transform target;     // For camera movement
    public float duration;       // Movement or wait duration
    public Animator animator;    // For PlayAnimation
    public string animationName; // Animation to play
}

public enum CutsceneEventType
{
    MoveCamera,
    PlayAnimation,
    Wait
}
