using UnityEngine;
using System.Collections;

public class SkyboxTinter : MonoBehaviour
{
    public static bool isDegenMode = false; // Static variable to toggle tint

    public Color redTint = new Color(1f, 0.3f, 0.3f, 1f);  // Slightly red
    public Color greenTint = new Color(0.3f, 1f, 0.3f, 1f); // Slightly green

    private Material skyboxMaterial;
    private bool previousDegenMode;

    void Start()
    {
        StartCoroutine(ToggleSkybox());

        // Try to find the Skybox component on the Main Camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Skybox skybox = mainCamera.GetComponent<Skybox>();
            if (skybox != null && skybox.material != null)
            {
                skyboxMaterial = skybox.material;
            }
        }

        // If still null, check RenderSettings
        if (skyboxMaterial == null && RenderSettings.skybox != null)
        {
            skyboxMaterial = RenderSettings.skybox;
        }

        if (skyboxMaterial == null)
        {
            Debug.LogError("No skybox material found! Make sure a skybox is assigned in the Main Camera or RenderSettings.");
        }
        else
        {
            // Initialize previousDegenMode and set initial skybox color
            previousDegenMode = isDegenMode;
            SetSkyboxColor(isDegenMode ? redTint : greenTint);
        }
    }

    void Update()
    {
        if (skyboxMaterial != null && isDegenMode != previousDegenMode)
        {
            // Update the skybox color instantly when isDegenMode changes
            SetSkyboxColor(isDegenMode ? redTint : greenTint);
            previousDegenMode = isDegenMode;
        }
    }

    void SetSkyboxColor(Color color)
    {
        skyboxMaterial.SetColor("_SkyTint", color);
        skyboxMaterial.SetColor("_GroundColor", color);
    }

    IEnumerator ToggleSkybox()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Wait for 10 seconds
            isDegenMode = !isDegenMode; // Toggle the boolean
            Debug.Log("Boolean State: " + isDegenMode);
        }
    }
}
