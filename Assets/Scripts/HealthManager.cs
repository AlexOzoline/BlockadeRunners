// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class HealthManager : MonoBehaviour
{    
    public float health = 10;

    [SerializeField] private int scoreReward = 10;

    [SerializeField] private GameObject explosionPreFab;
    
    public void setScoreReward(int score) {
        this.scoreReward = score;
    }

    public void setHealth(float healthVal) {
        health = healthVal;
    }

    public void damage(float damageAmount) {
        health -= damageAmount;
        
        if (health <= 0) {
            
            if (explosionPreFab != null) {
                var explosion = Instantiate(explosionPreFab);
                explosion.transform.position = this.transform.position;
            }

            Destroy(this.gameObject);

            GameManager.score += scoreReward;

            // Add in explosion
            
        }
    }

    // Used to destroy object in single hit
    public void oneShot() {
        if (explosionPreFab != null) {
                var explosion = Instantiate(explosionPreFab);
                explosion.transform.position = this.transform.position;
            }

            Destroy(this.gameObject);
    }
    
}
