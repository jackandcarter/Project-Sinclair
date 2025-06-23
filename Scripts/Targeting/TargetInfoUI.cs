using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays information about the currently selected <see cref="Targetable"/>.
/// Listens to <see cref="TargetManager"/> events and updates UI elements.
/// </summary>
public class TargetInfoUI : MonoBehaviour
{
    [Tooltip("Manager providing target selection events.")]
    public TargetManager targetManager;
    [Tooltip("Text element used to display the target name.")]
    public TextMeshProUGUI nameText;
    [Tooltip("Image fill used to display health remaining.")]
    public Image healthBar;

    private Targetable current;

    private void Awake()
    {
        if (targetManager == null)
        {
            targetManager = FindObjectOfType<TargetManager>();
        }
    }

    private void OnEnable()
    {
        if (targetManager != null)
        {
            targetManager.TargetSelected += OnTargetSelected;
            targetManager.TargetDeselected += OnTargetDeselected;
        }
    }

    private void OnDisable()
    {
        if (targetManager != null)
        {
            targetManager.TargetSelected -= OnTargetSelected;
            targetManager.TargetDeselected -= OnTargetDeselected;
        }
    }

    private void Update()
    {
        if (current != null && healthBar != null)
        {
            healthBar.fillAmount = current.maxHP > 0 ? (float)current.currentHP / current.maxHP : 0f;
        }
    }

    private void OnTargetSelected(Targetable t)
    {
        current = t;
        if (nameText != null)
        {
            nameText.text = t != null ? t.displayName : string.Empty;
        }
        gameObject.SetActive(t != null);
    }

    private void OnTargetDeselected(Targetable t)
    {
        if (current == t)
        {
            current = null;
            gameObject.SetActive(false);
        }
    }
}
