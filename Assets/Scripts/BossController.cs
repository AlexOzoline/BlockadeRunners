using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    
    public float timeAtSpawn = 0f;

    // Move speed
    public float speed = 30f;

    //private float period = 1f;
    //private float crestHeight = 1f;
    
    // Normalized vector for laser direction
    [SerializeField] private Vector3 direction = new Vector3(1, 0, 0);
    // Vector is 2D - not using Z dimension

    // Direction vector towards player
    [SerializeField] private Vector3 aimDirection = Vector3.left;

    // Direction vector perpendicular to aimDirection
    [SerializeField] private Vector3 perpDirection = Vector3.up;

    [SerializeField] private float playerDistance = 0f;

    [SerializeField] private float collisionDamage = 10;
    [SerializeField] private float cannonDamage = 10;

    [SerializeField] private ProjectileController projectilePrefab;

    //[SerializeField] private GameObject explosion; // BOOM!

    // Store reference to player for ease of access
    private PlayerController player;

    [SerializeField] private float lowerDist = 1f;
    [SerializeField] private float upperDist = 3f;

    private Vector3 movementDirection = Vector3.zero;

    [SerializeField] private float xMax = 13f;
    [SerializeField] private float yMax = 4.5f;

    private float sideDirection = 1;


    public void Initialize(float time, PlayerController playerObj, int bossNum) {
        timeAtSpawn = (time);
        
        this.speed = (timeAtSpawn) / 6f;
        this.cannonDamage = (timeAtSpawn) / 9f;

        transform.position = (new Vector3(GameManager.xMax + 5, 0, 0));

        // Scale health using timeAtSpawn
        var healthManager = this.gameObject.GetComponent<HealthManager>();
        if (healthManager != null) {
            healthManager.setHealth((float)bossNum * 10f);
            healthManager.setScoreReward(bossNum * 50);
        }

        this.player = playerObj;

        StartCoroutine(ShootCoroutine());

        this.xMax = GameManager.xMax;
        this.yMax = GameManager.yMax;
    }
    
    // https://docs.unity3d.com/Manual/ExecutionOrder.html
    private void Update()
    {   

        // Aim at player
        aimAtPlayer();

        // Transform based on player location
        // Move towards/away from player to stay in set range (use aimDirection)

        if (playerDistance < lowerDist) { // Move away from player
            movementDirection += (aimDirection * -1);

        } else if (playerDistance > upperDist) { // Move towards player
            movementDirection += (aimDirection);

        } else { // Randomly move forward/back
            // movementDirection will be normalized, so halving the vector here won't change that
            // butt will weight movement towards player less than movement perpendicular to player in this case
            movementDirection += (aimDirection * Random.Range(-0.5f, 0.5f));
        }

        // Move randomly perpendicular to player
        movementDirection += (perpDirection * Random.Range(0f, 0.75f) * sideDirection);

        if (Random.Range(0f, 1f) < 0.05) {
            sideDirection *= -1;
        }
 
        movementDirection.Normalize(); // = movementDirection.normalized;

        // Avoid going out of bounds - use same logic as for player
        if (transform.position.x > xMax && movementDirection.x > 0) {
            movementDirection.x = 0;
        }
        if (transform.position.x < xMax * -1 && movementDirection.x < 0) {
            movementDirection.x = 0;
        }
        if (transform.position.y > yMax && movementDirection.y > 0) {
            movementDirection.y = 0;
        }
        if (transform.position.y < yMax * -1 && movementDirection.y < 0) {
            movementDirection.y = 0;
        }


        transform.Translate( movementDirection * (speed * Time.deltaTime), Space.World);

    }

    private void OnTriggerEnter(Collider other){
        // Just get other collider, not collision information

        // Just check collision w/ player
        var playerHealthManager = other.GetComponent<PlayerHealthManager>();

        if (playerHealthManager != null) {
            playerHealthManager.damage(collisionDamage);
            this.gameObject.GetComponent<HealthManager>().damage(10);
        }

        // Destroy(this.gameObject);
        // Use particle system to have blast using explosion

        
    }

    IEnumerator ShootCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.2f, 1f));

            ProjectileController projectile = Instantiate(projectilePrefab);

            projectile.Initialize(cannonDamage, speed + 10f, aimDirection);

            // set position
            projectile.transform.position = transform.position; // + new Vector3(-1.25f ,0 ,0);
        }
    }

    private void aimAtPlayer() {

        //var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //var gamePlane = new Plane(Vector3.back, Vector3.zero);

        if (player != null) {

            // Get vector b/t boss and player positions
            aimDirection = (player.transform.position - this.transform.position);

            // Use aiming vector to get distance
            playerDistance = aimDirection.magnitude;

            // Normalize aiming vector
            aimDirection = aimDirection.normalized;

            // Take the cross product of aimDirection and Vector3.forward to get perpendicular vector
            perpDirection = (Vector3.Cross(aimDirection, Vector3.forward)).normalized;

            //this.transform.rotation = Quaternion.LookRotation(aimDirection, Vector3.forward);
            this.transform.LookAt(player.transform.position, Vector3.back);
        }

        
    }
}
