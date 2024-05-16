using UnityEngine;
using System.Collections;
public class RotateIcon : MonoBehaviour
{
    public float rotationSpeed = 360f; // Speed of rotation in degrees per second
    public float rotationDuration = 1f; // Duration of rotation in seconds

    private bool isRotating = false; // Flag to check if rotation is in progress

    public void StartRotation()
    {
        if (!isRotating)
        {
            isRotating = true;
            StartCoroutine(RotateCoroutine());
        }
    }

    private IEnumerator RotateCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRotating = false;
    }
}
