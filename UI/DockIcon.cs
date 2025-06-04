using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DockIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Common Settings")]
    [Tooltip("If true, this icon cannot be dragged, rearranged, or removed.")]
    public bool isStatic = false; 

    [Header("Dock Icon Additional Settings")]
    [Tooltip("If false, dropping outside the dock does not remove/destroy this icon; it snaps back instead.")]
    public bool isRemovable = true;

    [Tooltip("If true, dragging this icon spawns a copy (shortcut). The original remains in place.")]
    public bool isCopyable = false;

    // Used internally to differentiate an original icon from a spawned copy.
    private bool isClone = false;

    [Header("Drag Settings")]
    public Canvas parentCanvas; // If not set, auto-finds the parent Canvas.
    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;
    protected Vector2 originalPosition;
    protected Transform originalParent;

    [Header("Ability Activation Settings")]
    public float abilityCastTime = 2f;
    public float abilityRiseHeight = 50f; // Pixels to raise during ability activation.
    public float abilityFallSpeed = 100f; // Pixels per second while falling back.
    protected bool isAbilityActive = false;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (parentCanvas == null)
            parentCanvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent;
    }

    protected virtual void Update()
    {
        // No magnification logic here for now.
    }

    ////////////////
    // DRAG & DROP LOGIC
    ////////////////

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (isStatic)
            return; // Do not allow dragging for static icons.

        // If this icon is copyable and we're not already a clone, spawn a copy and drag that instead.
        if (isCopyable && !isClone)
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            DockIcon cloneIcon = clone.GetComponent<DockIcon>();

            // The clone shouldn't keep duplicating itself.
            cloneIcon.isCopyable = false;
            cloneIcon.isClone = true;

            // Switch the pointer to drag the clone.
            eventData.pointerDrag = clone;
            // (Optionally, hide or disable the original here if desired.)
            return;
        }

        // Otherwise, proceed with normal drag of this icon.
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        // Move to top-level canvas so it's not clipped during drag.
        transform.SetParent(parentCanvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (isStatic)
            return;

        // Move with the mouse, factoring in canvas scale.
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (isStatic)
            return;

        canvasGroup.blocksRaycasts = true;

        // If still parented to the top-level canvas, then no valid drop zone accepted it.
        if (transform.parent == parentCanvas.transform)
        {
            // Check if this is a folder.
            DockFolder folder = GetComponent<DockFolder>();
            if (folder != null)
            {
                // If folder is permanent, snap back.
                if (folder.isPermanent)
                {
                    transform.SetParent(originalParent);
                    rectTransform.anchoredPosition = originalPosition;
                }
                else
                {
                    // For non‑permanent folders, show a removal prompt.
                    RemovalPrompt.Instance.ShowPrompt("Remove folder?", () =>
                    {
                        // On confirm, remove this folder.
                        Destroy(gameObject);
                    },
                    () =>
                    {
                        // On cancel, snap back to its original position.
                        transform.SetParent(originalParent);
                        rectTransform.anchoredPosition = originalPosition;
                    });
                }
            }
            else
            {
                // Regular icon logic.
                if (!isRemovable)
                {
                    // If icon isn't removable, snap back to original spot.
                    transform.SetParent(originalParent);
                    rectTransform.anchoredPosition = originalPosition;
                }
                else
                {
                    // If removable, destroy it.
                    Destroy(gameObject);
                }
            }
            return;
        }

        // If dropped in a valid drop zone, reorder icons in that container if it has a DockManager.
        DockManager dm = transform.parent.GetComponent<DockManager>();
        if (dm != null)
            dm.ReorderIcons();
    }

    ////////////////
    // ABILITY ACTIVATION (for non-folder icons)
    ////////////////

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // If this is not a folder and not currently animating, activate ability.
        if (GetComponent<DockFolder>() == null && !isAbilityActive)
            StartCoroutine(AbilityActivationRoutine());
    }

    protected virtual IEnumerator AbilityActivationRoutine()
    {
        isAbilityActive = true;
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0, abilityRiseHeight);
        float elapsed = 0f;

        // Move upward during cast time.
        while (elapsed < abilityCastTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / abilityCastTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;

        // (Insert ability activation logic here.)

        // Fall back down.
        while (rectTransform.anchoredPosition.y > startPos.y)
        {
            rectTransform.anchoredPosition -= new Vector2(0, abilityFallSpeed * Time.deltaTime);
            if (rectTransform.anchoredPosition.y < startPos.y)
                rectTransform.anchoredPosition = startPos;
            yield return null;
        }
        isAbilityActive = false;
    }
}
