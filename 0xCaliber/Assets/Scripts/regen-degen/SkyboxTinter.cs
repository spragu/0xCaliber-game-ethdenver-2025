using UnityEngine;
using System.Collections;

public class SkyboxTinter : MonoBehaviour
{
    public static bool isDegenMode = false; // Static variable to toggle tint

    public Color redTint = new Color(1f, 0.3f, 0.3f, 1f);  // Slightly red
    public Color greenTint = new Color(0.3f, 1f, 0.3f, 1f); // Slightly green
    public float lerpSpeed = 2.0f; // Speed of color change

    private Material skyboxMaterial;

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
    }

    void Update()
    {

        
        if (skyboxMaterial != null)
        {
            Color targetColor = isDegenMode ? redTint : greenTint;
            Color currentSkyTint = skyboxMaterial.GetColor("_SkyTint");
            Color currentGroundColor = skyboxMaterial.GetColor("_GroundColor");

            // Lerp for smooth transition
            skyboxMaterial.SetColor("_SkyTint", Color.Lerp(currentSkyTint, targetColor, Time.deltaTime * lerpSpeed));
            skyboxMaterial.SetColor("_GroundColor", Color.Lerp(currentGroundColor, targetColor, Time.deltaTime * lerpSpeed));
        }
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
