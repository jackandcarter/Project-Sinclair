using UnityEngine;

/// <summary>
/// Makes the camera smoothly follow a target with a constant offset.
/// </summary>
[RequireComponent(typeof(Camera))]
public class TopDownCameraFollow : MonoBehaviour
{
    /// <summary>
    /// Transform that the camera will follow.
    /// </summary>
    public Transform target;

    /// <summary>
    /// Offset from the target position.
    /// </summary>
    public Vector3 offset = new Vector3(0f, 10f, -10f);

    /// <summary>
    /// Time it takes to smooth towards the target.
    /// </summary>
    public float smoothTime = 0.2f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        float t = Time.deltaTime / Mathf.Max(smoothTime, 0.0001f);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, t);
    }
}
