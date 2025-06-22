using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private DeveloperOptions developerOptions;
    [SerializeField] public GameObject[] spawnableObjects;
    [SerializeField] private GameObject axisArrowPrefab;

    private int currentIndex = 0;
    private GameObject currentSpawnedObject;
    private GameObject placementIndicator;
    private GameObject[] axisArrows;

    void Update()
    {
        // Check if dropdown is open before attempting to spawn
        if (!developerOptions.IsDropdownOpen())
            return;

        // Check if left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            if (currentSpawnedObject == null)
            {
                SpawnObject();
            }
            else
            {
                EndPlacement();
            }
        }

        // Update placement indicator position during placement
        if (currentSpawnedObject != null)
        {
            UpdatePlacementIndicatorPosition();
        }
    }

    public void SetCurrentIndex(int index)
    {
        currentIndex = index;
    }

    private void SpawnObject()
    {
        Vector3 spawnPosition;

        // Check if the mouse is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Spawn object in front of player
            spawnPosition = player.transform.position + player.transform.forward * 2;
        }
        else
        {
            // Use raycast to place object on terrain
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                spawnPosition = hit.point;
            }
            else
            {
                return;
            }
        }

        // Ensure currentIndex is within bounds
        if (currentIndex >= 0 && currentIndex < spawnableObjects.Length)
        {
            currentSpawnedObject = Instantiate(spawnableObjects[currentIndex], spawnPosition, Quaternion.identity);
            // Activate placement indicator and axis arrows
            ActivatePlacementIndicator(spawnPosition);
            ActivateAxisArrows(currentSpawnedObject.transform.position);
        }
        else
        {
            Debug.LogWarning("Invalid currentIndex for spawning.");
        }
    }

    private void EndPlacement()
    {
        // Destroy placement indicator and axis arrows
        if (placementIndicator != null)
        {
            Destroy(placementIndicator);
        }
        if (axisArrows != null)
        {
            foreach (GameObject arrow in axisArrows)
            {
                Destroy(arrow);
            }
        }

        // Reset currentSpawnedObject
        currentSpawnedObject = null;
    }

    private void ActivatePlacementIndicator(Vector3 position)
    {
        // Instantiate or move placement indicator
        if (placementIndicator == null)
        {
            placementIndicator = Instantiate(spawnableObjects[currentIndex], position, Quaternion.identity);
            // Add transparency or shader for indication
        }
        else
        {
            placementIndicator.transform.position = position;
        }
    }

    private void ActivateAxisArrows(Vector3 position)
    {
        // Instantiate axis arrows at the position of the spawned object
        axisArrows = new GameObject[3];

        // Create arrow objects
        for (int i = 0; i < 3; i++)
        {
            // Instantiate arrow prefab and position it accordingly
            axisArrows[i] = Instantiate(axisArrowPrefab, position, Quaternion.identity);
            // Set up event listeners for arrow manipulation
            int axisIndex = i;
            axisArrows[i].GetComponent<AxisArrow>().OnArrowClicked.AddListener(() => ManipulateObject(axisIndex));
        }
    }

    private void UpdatePlacementIndicatorPosition()
    {
        // Update placement indicator position based on mouse cursor's position or raycast hit position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            placementIndicator.transform.position = hit.point;
        }
    }

    private void ManipulateObject(int axis)
    {
        // Move the object along the specified axis
        Vector3 direction = Vector3.zero;
        direction[axis] = 1; // Set the direction along the specified axis

        // Calculate new position for the object
        Vector3 newPosition = currentSpawnedObject.transform.position + direction;

        // Move the object to the new position
        currentSpawnedObject.transform.position = newPosition;
    }
}
