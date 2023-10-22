using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Move speed for the laser shot
    public float speed = 30f;
    
    // Normalized vector for laser direction
    public Vector3 direction = new Vector3(1, 0, 0);
    // Vector is 2D - not using Z dimension

    public float damage = 5;

    // Don't need, using layers now
    //public bool shotByPlayer = false;

    [SerializeField] private GameObject splashPreFab;

    
    public void Initialize(float damage, float speed, Vector3 direction){
        this.damage = damage;
        this.speed = speed;
        this.direction = direction.normalized;
    }

    
    // https://docs.unity3d.com/Manual/ExecutionOrder.html
    private void Update()
    {   
        transform.Translate( direction * (this.speed * Time.deltaTime));

        
        var BoundCheck = this.gameObject.GetComponent<BoundryCheck>();
        if (BoundCheck != null) {
            BoundCheck.checkBound();
        }
        

    }

    private void OnTriggerEnter(Collider other){
        // Just get other collider, not collision information

        /*
        if (!shotByPlayer) {
            // Handles collisions w/ both player and non-players
            var healthManager = other.GetComponent<HealthManager>();
            var playerHealthManager = other.GetComponent<PlayerHealthManager>();

            if (healthManager != null) {
                healthManager.damage(damage);
            } else if (playerHealthManager != null) {
                playerHealthManager.damage(damage);
            }
        } else { // ShotByPlayer == true
            // Shot by player, so don't check for collisions w/ player
            var healthManager = other.GetComponent<HealthManager>();

            if (healthManager != null) {
                healthManager.damage(damage);
            }
        }
        */

        var healthManager = other.GetComponent<HealthManager>();
        var playerHealthManager = other.GetComponent<PlayerHealthManager>();

        if (healthManager != null) {
            healthManager.damage(damage);
        } else if (playerHealthManager != null) {
            playerHealthManager.damage(damage);
        }

        if (splashPreFab != null) {
            var particles = Instantiate(this.splashPreFab);
            particles.transform.position = transform.position;
            particles.transform.rotation =
                Quaternion.LookRotation(direction * -1);
        }

        Destroy(this.gameObject);
    }
}
