using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class SmallEnemyController : MonoBehaviour
{
    // UNIFINISHED
    
    public float timeAtSpawn = 0f;

    // Move speed
    public float crossSpeed = 30f;

    private float period = 1f;
    private float crestHeight = 1f;
    
    // Normalized vector for laser direction
    [SerializeField] private Vector3 direction = new Vector3(1, 0, 0);
    // Vector is 2D - not using Z dimension

    [SerializeField] private float collisionDamage = 10;
    [SerializeField] private float cannonDamage = 10;

    [SerializeField] private ProjectileController projectilePrefab;

    //[SerializeField] private GameObject explosion; // BOOM!


    /*
    private void Start() {
        timeAtSpawn = Time.fixedTime;

        // Set speed using play time - scales up slightly faster than for asteroids
        //this.crossSpeed = timeAtSpawn * 3;

        //this.sinOffset = 

        // Set cannon damage based on time
        //this.cannonDamage = timeAtSpawn * 0.5f;

        // Randomize Direction
        //direction = new Vector3(-1f, Random.Range(-5f, 5f), 0);
        //direction = direction.normalized;

        // Set random start location to right of screen
        transform.localPosition = new Vector3(15f, Random.Range(-4f, 4f), 0);

        this.period = Random.Range(0.5f, 3f);
        this.crestHeight = Random.Range(0f, 1f);

    }
    */

    public void Initialize(float time) {
        timeAtSpawn = Mathf.Sqrt(time);
        this.crossSpeed = (timeAtSpawn) * -2;
        this.cannonDamage = (timeAtSpawn) * 0.5f;

        transform.position = (new Vector3(GameManager.xMax + 5, Random.Range(GameManager.yMax * -1, GameManager.yMax), 0));
        this.period = Random.Range(0.1f, 1f);
        this.crestHeight = Random.Range(-1f, 1f);

        // Scale health using timeAtSpawn
        var healthManager = this.gameObject.GetComponent<HealthManager>();
        if (healthManager != null) {
            healthManager.setHealth(timeAtSpawn/10f);
            healthManager.setScoreReward((int)timeAtSpawn * 3);
        }

        StartCoroutine(ShootCoroutine());
    }
    
    // https://docs.unity3d.com/Manual/ExecutionOrder.html
    private void Update()
    {   
        // Transform based on set equation - sin - so ship moves back and forth
        //transform.Translate( direction * (this.speed * Time.deltaTime));
        transform.Translate(new Vector3(crossSpeed, crestHeight * Mathf.Sin(Time.fixedTime * period), 0) * Time.deltaTime, Space.World);
        
        this.gameObject.GetComponent<BoundryCheck>().checkLeftBound();

        // Shoot periodically
        // Scale projectile speed w/ enemy speed, set projectile damage
        // Make sure travelling to the left (negative x-direction)
        /*
        if (Random.Range(0f, 1f) < 0.1f) {
            ProjectileController projectile = Instantiate(projectilePrefab);

            projectile.Initialize(cannonDamage, crossSpeed - 10f, Vector3.left);

            // Start offset
            projectile.transform.localPosition = transform.localPosition + new Vector3(-1.25f ,0 ,0);
        }
        */

    }

    private void OnTriggerEnter(Collider other){
        // Just get other collider, not collision information

        // Just check collision w/ player
        var playerHealthManager = other.GetComponent<PlayerHealthManager>();

        if (playerHealthManager != null) {
            playerHealthManager.damage(collisionDamage);
            this.gameObject.GetComponent<HealthManager>().damage(collisionDamage);

        }

        //Destroy(this.gameObject);
        // Use particle system to have blast using explosion
    }

    IEnumerator ShootCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.25f, 1f));

            ProjectileController projectile = Instantiate(projectilePrefab);

            projectile.Initialize(cannonDamage, (crossSpeed * -1) + 15f, Vector3.left);

            // Start offset
            projectile.transform.position = transform.position + new Vector3(-1.25f ,0 ,0);
        }
    }
}
