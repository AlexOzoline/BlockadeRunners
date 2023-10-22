using UnityEngine;

public class AsteroidController : MonoBehaviour
{

    public float timeAtSpawn = 0f;

    // Move speed for the laser shot
    public float speed = 30f;
    
    // Normalized vector for laser direction
    [SerializeField] private Vector3 direction = new Vector3(1, 0, 0);
    // Vector is 2D - not using Z dimension

    public float damage = 10;

    //[SerializeField] private GameObject AsteroidBlast;

    [SerializeField] private Vector3 rotation = new Vector3(0f,0f,0f);

    /*
    private void Start() {

        timeAtSpawn = Time.fixedTime;

        // Set speed using play time
        //this.speed = Time.fixedTime * 2;

        // Randomize scale
        transform.localScale = new Vector3(Random.Range(0.5f, 5f), Random.Range(0.5f, 5f), Random.Range(0.5f, 5f));

        // Set damage based on size - larger asteroids will do more damage
        this.damage = transform.localScale.magnitude;

        // Randomize rotation
        rotation = new Vector3(Random.Range(0f, 30f), Random.Range(0f, 30f), Random.Range(0f, 30f));

        // Randomize Direction
        direction = new Vector3(-1f, Random.Range(-5f, 5f), 0);
        direction = direction.normalized;

        // Set random start location to right of screen
        transform.localPosition = new Vector3(15f, Random.Range(-4f, 4f), 0);

    }
    */

    public void Initialize(float time, float playerLane) {
        timeAtSpawn = Mathf.Sqrt(time + 10);

        transform.localScale = new Vector3(Random.Range(50f, 100f), Random.Range(50f, 100f), Random.Range(50f, 100f));

        this.speed = timeAtSpawn * 0.5f;

        direction = new Vector3(-1f, Random.Range(-0.5f, 0.5f), 0);
        direction = direction.normalized;

        rotation = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));

        //this.damage = transform.localScale.magnitude / 100;
        this.gameObject.GetComponent<HealthManager>().setScoreReward((int)timeAtSpawn);

        
        // Randomness will skip over the player lane
        // Player lane is 1/5 of the full height of the game screen
        float laneWidth = GameManager.yMax * 2 / 5;
        float ySpawn = Random.Range(-1 * GameManager.yMax, GameManager.yMax - laneWidth);
        if (ySpawn >= playerLane - laneWidth/2) {
            ySpawn += laneWidth;
        }
        transform.SetPositionAndRotation(new Vector3(GameManager.xMax + 5, ySpawn, 0), Quaternion.identity);

    }
    
    // https://docs.unity3d.com/Manual/ExecutionOrder.html
    private void Update()
    {   
        transform.Translate( direction * (speed * Time.deltaTime), Space.World);

        this.gameObject.transform.Rotate(rotation, Space.World);
        
        this.gameObject.GetComponent<BoundryCheck>().checkLeftBound();

    }


        
    private void OnTriggerEnter(Collider other){
        // Just get other collider, not collision information

        // Just check collision w/ player
        var playerHealthManager = other.GetComponent<PlayerHealthManager>();
        //var projectileController = other.GetComponent<ProjectileController>();

        if (playerHealthManager != null) {
            playerHealthManager.damage(damage);
            //print("Asteroid impact");
            //Destroy(this.gameObject);

            this.gameObject.GetComponent<HealthManager>().oneShot();

        }
        /*
        if (projectileController != null) {
            print("Asteroid hit by laser");
            this.gameObject.GetComponent<HealthManager>().damage(projectileController.damage);
            Destroy(other.gameObject);
        }
        */

        
        // Use particle system to have blast using AsteroidBlast
    }
    
    
}
