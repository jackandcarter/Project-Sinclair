using UnityEngine;
using UnityEngine.EventSystems;

public class DockFolder : DockIcon, IDropHandler {

    [Header("Folder Specific Settings")]
    [Tooltip("The sub-container (child panel) that holds this folder’s items.")]
    public RectTransform subContainer;
    [Tooltip("If true, this folder is permanent (built-in) and cannot be removed or rearranged.")]
    public bool isPermanent = false;

    protected override void Awake() {
        base.Awake();
        if (subContainer == null)
        {
            GameObject child = new GameObject("SubDock", typeof(RectTransform), typeof(DockManager));
            RectTransform rt = child.GetComponent<RectTransform>();
            rt.SetParent(transform, false);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            subContainer = rt;
            child.SetActive(false);
        }
        if (isPermanent) {
            isStatic = true;
        }
    }

    public override void OnPointerClick(PointerEventData eventData) {
        ToggleFolder();
        Execute();
    }

    public void ToggleFolder() {
        if (subContainer != null) {
            bool active = subContainer.gameObject.activeSelf;
            subContainer.gameObject.SetActive(!active);
            // Optionally reorder items within the folder when opened.
            DockManager subManager = subContainer.GetComponent<DockManager>();
            if (subManager != null)
                subManager.ReorderIcons();
        }
    }

    // Accepts dropped icons and reorders them inside the folder.
    public void OnDrop(PointerEventData eventData) {
        DockIcon droppedIcon = eventData.pointerDrag.GetComponent<DockIcon>();
        if (droppedIcon != null && subContainer != null) {
            droppedIcon.transform.SetParent(subContainer);
            DockManager subManager = subContainer.GetComponent<DockManager>();
            if (subManager != null)
                subManager.ReorderIcons();
        }
    }
}
