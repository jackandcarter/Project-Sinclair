using UnityEngine;
using UnityEngine.Events;

public class AxisArrow : MonoBehaviour
{
    public UnityEvent OnArrowClicked;

    private void OnMouseDown()
    {
        OnArrowClicked?.Invoke();
    }
}
