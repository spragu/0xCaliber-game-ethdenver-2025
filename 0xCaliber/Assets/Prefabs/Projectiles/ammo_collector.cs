using UnityEngine;
using Fusion;
public class ammo_collector : MonoBehaviour
{

    private bool pickedUp = false;
    private void OnTriggerEnter(Collider other)
    {
        if(pickedUp)
            return;
        Debug.Log("Touching powerup");
        Destroy(gameObject);
        multiplayerState.ammoCount++;

    }
}
