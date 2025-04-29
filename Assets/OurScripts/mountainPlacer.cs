using UnityEngine;

[ExecuteInEditMode]
public class MountainPlacer : MonoBehaviour
{
    [Header("Mountain Settings")]
    public GameObject mountainPrefab; // Your sprite prefab with the PNG
    public int numberOfMountains = 12;
    public float baseRadius = 100f;
    public float radiusVariation = 10f;
    public Vector2 baseScale = new Vector2(50f, 30f);
    public Vector2 scaleVariation = new Vector2(10f, 5f);
    public float baseHeight = -10f;
    public float heightVariation = 5f;
    public float rotationOffset = 0f;

    [Header("Placement Controls")]
    public bool updateInRealTime = false;
    public bool generateMountains = false;
    public bool clearMountains = false;

    private GameObject[] mountainInstances;

    void Update()
    {
        // Editor controls for easy tweaking
        if (updateInRealTime && mountainInstances != null && mountainInstances.Length > 0)
        {
            UpdateMountainPlacement();
        }

        if (generateMountains)
        {
            generateMountains = false;
            GenerateMountains();
        }

        if (clearMountains)
        {
            clearMountains = false;
            ClearMountains();
        }
    }

    public void GenerateMountains()
    {
        // Clear existing mountains if any
        ClearMountains();

        mountainInstances = new GameObject[numberOfMountains];

        for (int i = 0; i < numberOfMountains; i++)
        {
            // Calculate angle around the circle
            float angle = i * Mathf.PI * 2f / numberOfMountains + rotationOffset * Mathf.Deg2Rad;

            // Calculate position with some variation
            float radius = baseRadius + Random.Range(-radiusVariation, radiusVariation);
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                baseHeight + Random.Range(-heightVariation, heightVariation),
                Mathf.Sin(angle) * radius
            );

            // Create mountain instance
            GameObject mountain = Instantiate(mountainPrefab, position, Quaternion.identity, transform);
            
            // Set scale with some variation
            Vector3 scale = new Vector3(
                baseScale.x + Random.Range(-scaleVariation.x, scaleVariation.x),
                baseScale.y + Random.Range(-scaleVariation.y, scaleVariation.y),
                1f
            );
            mountain.transform.localScale = scale;

            // Face the mountain towards the center
            mountain.transform.LookAt(transform.position);
            // Keep the mountain upright (adjust if needed)
            mountain.transform.rotation = Quaternion.Euler(0f, mountain.transform.rotation.eulerAngles.y + 180f, 0f);

            mountainInstances[i] = mountain;
        }
    }

    public void UpdateMountainPlacement()
    {
        if (mountainInstances == null || mountainInstances.Length != numberOfMountains)
        {
            GenerateMountains();
            return;
        }

        for (int i = 0; i < numberOfMountains; i++)
        {
            if (mountainInstances[i] == null) continue;

            float angle = i * Mathf.PI * 2f / numberOfMountains + rotationOffset * Mathf.Deg2Rad;
            float radius = baseRadius + Random.Range(-radiusVariation, radiusVariation);
            
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                baseHeight + Random.Range(-heightVariation, heightVariation),
                Mathf.Sin(angle) * radius
            );

            mountainInstances[i].transform.position = position;
            
            Vector3 scale = new Vector3(
                baseScale.x + Random.Range(-scaleVariation.x, scaleVariation.x),
                baseScale.y + Random.Range(-scaleVariation.y, scaleVariation.y),
                1f
            );
            mountainInstances[i].transform.localScale = scale;

            mountainInstances[i].transform.LookAt(transform.position);
            mountainInstances[i].transform.rotation = Quaternion.Euler(0f, mountainInstances[i].transform.rotation.eulerAngles.y + 180f, 0f);
        }
    }

    public void ClearMountains()
    {
        if (mountainInstances != null)
        {
            foreach (var mountain in mountainInstances)
            {
                if (mountain != null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(mountain);
                    }
                    else
                    {
                        DestroyImmediate(mountain);
                    }
                }
            }
        }

        // Also destroy any remaining children (manual deletions)
        while (transform.childCount > 0)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        mountainInstances = null;
    }
}