using UnityEngine;

/// <summary>
/// Creates a circular trigger that keeps combatants inside the battle area.
/// Characters attempting to leave are pushed back with a force.
/// </summary>
public class BattleBarrier : MonoBehaviour
{
    /// <summary>
    /// Radius of the barrier in world units.
    /// </summary>
    public float radius = 10f;

    /// <summary>
    /// Height of the visual mesh.
    /// </summary>
    public float height = 2f;

    /// <summary>
    /// Force applied to characters trying to exit.
    /// </summary>
    public float pushForce = 15f;

    /// <summary>
    /// Normal barrier color.
    /// </summary>
    public Color neutralColor = Color.cyan;

    /// <summary>
    /// Color when a character collides with the barrier while trying to escape.
    /// </summary>
    public Color escapeColor = Color.red;

    private Material runtimeMaterial;
    private Renderer rend;

    private void Start()
    {
        SphereCollider collider = gameObject.GetComponent<SphereCollider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<SphereCollider>();
        }
        collider.isTrigger = true;
        collider.radius = radius;

        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Destroy(visual.GetComponent<Collider>());
            visual.transform.SetParent(transform, false);
            visual.transform.localScale = new Vector3(radius * 2f, height * 0.5f, radius * 2f);
            rend = visual.GetComponent<Renderer>();
        }

        if (rend != null)
        {
            runtimeMaterial = new Material(rend.material);
            rend.material = runtimeMaterial;
            runtimeMaterial.color = neutralColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            rb.AddForce(dir * pushForce, ForceMode.VelocityChange);
            SetColor(escapeColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SetColor(neutralColor);
    }

    private void SetColor(Color color)
    {
        if (runtimeMaterial != null)
        {
            runtimeMaterial.color = color;
        }
    }
}

