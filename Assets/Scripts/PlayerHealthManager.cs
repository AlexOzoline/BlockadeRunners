// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{    
    public float health = 20;

    public float maxHealth = 20;

    public int shieldCapacity = 0;

    [SerializeField] private GameObject playerExplosion;
    
    public void Initialize(float health){
        this.health = health;
        this.maxHealth = health;

        GameObject healthBarObj = GameObject.Find("HealthBar");
        HealthBar healthBarComponent = healthBarObj.GetComponent<HealthBar>();
        healthBarComponent.SetHealth(health);
    }

    public void damage(float damageAmount) {

        if (shieldCapacity <= 0) {
            health -= damageAmount;
            GameObject healthBarObj = GameObject.Find("HealthBar");
            HealthBar healthBarComponent = healthBarObj.GetComponent<HealthBar>();
            healthBarComponent.SetHealth(health);
        } else {
            shieldCapacity--;
        }
        
        if (health <= 0) {

            // Destroy player - add in particle effects
            var explosion = Instantiate(playerExplosion);
            explosion.transform.position = this.transform.position;
            Destroy(this.gameObject);

            // Move to Game Over scene - pass over time from gameManager
            // Move over is now handled by GameManager
        }
    }

    public void heal(float healAmount) {
        this.health += healAmount;
        if (this.health > this.maxHealth) {
            this.health = this.maxHealth;
        }
        GameObject healthBarObj = GameObject.Find("HealthBar");
        HealthBar healthBarComponent = healthBarObj.GetComponent<HealthBar>();
        healthBarComponent.SetHealth(health);
    }

    public void addShield(int capacity) {
        shieldCapacity = capacity;
    }

    public int getShieldCapacity() {
        return shieldCapacity;
    }
    
}
