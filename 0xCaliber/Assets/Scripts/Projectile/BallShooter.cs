using UnityEngine;

public class BallShooter : MonoBehaviour
{
     // The projectile prefab should be a ball with a Rigidbody component.
    public GameObject projectilePrefab;
    
    // The force with which the projectile is shot.
    public float shootForce = 20f;
    
    // The lifetime (in seconds) after which the projectile is destroyed.
    public float projectileLifetime = 5f;

    // Update is called once per frame. Here we listen for input.
    void Update()
    {
        // This uses the default "Fire1" input (left mouse button or Ctrl by default).
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    // Fires the projectile.
    void Fire()
    {
        // Instantiate the projectile at the position and rotation of this GameObject.
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Get the Rigidbody component from the projectile.
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply an impulse force in the forward direction of the GameObject.
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Projectile prefab is missing a Rigidbody component.");
        }

        // Destroy the projectile after a delay to simulate it falling off.
        Destroy(projectile, projectileLifetime);
    }
}
