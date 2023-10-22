using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private Vector3 aimDirection = Vector3.right;
    public Renderer shieldRend;

    public int capacity = 0;
    void Start()
    {
        
    }

    void Update()
    {   
        
        // Conditionally turn off the shields when the player's shield health is 0

        // Check if the player has been destroyed
        var checkForShip = GameObject.Find("PlayerShip(Clone)");
        if(checkForShip == null) {
            return;
        }
        // Player must be still alive, continue as normal
        var playerHealthManager = GameObject.Find("PlayerShip(Clone)").GetComponent<PlayerHealthManager>();
        if (playerHealthManager != null) {
            shieldRend = GameObject.Find("Shields").GetComponent<Renderer>();
            this.capacity = playerHealthManager.shieldCapacity;
            if (capacity <= 0) {
                shieldRend.enabled = false;

            }
            else {
                shieldRend.enabled = true;
            }

            this.GetComponent<Renderer>().material.SetInt("_ShieldCapacity", capacity);
        }

        
        
        // Find ship direction, (using similar logic to the aim() function in playerController)
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var gamePlane = new Plane(Vector3.back, Vector3.zero);

        if (gamePlane.Raycast(mouseRay, out var distance)) {
            
            // Get hit point
            var hitPoint = mouseRay.GetPoint(distance);

            aimDirection = (hitPoint - gameObject.transform.position).normalized;
        }
        // Manually determined the length of the front of the ship that the shields need to be displaced towards to be centered
        var shipOffset = 0.3163621343f;

        // Fix the aimDirection length to be that of the offset
        aimDirection = aimDirection * shipOffset;

        // Move the shields to be ontop of the player
        GameObject window = GameObject.Find("window");
        if (window != null)
        {   
            Vector3 shipPosition = window.transform.position;
            transform.position = new Vector3 (shipPosition.x + aimDirection.x, shipPosition.y + aimDirection.y, shipPosition.z);
        }
    }
}
