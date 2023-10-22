using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private bool inertiaMovement = false;

    // max speed will be affected by engine type
    //[SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float maxSpeed = 10f;

    [SerializeField] private float acceleration = 1f;

    [SerializeField] private float damage = 5f;

    [SerializeField] private float fireSpeed = 1f;

    private float timeSinceLastShot = 0f;

    private Vector3 aimDirection = Vector3.right;

    //[SerializeField] private float health = 100;

    [SerializeField] private Vector3 movement = new Vector3(0,0,0);

    // Make sure is adjusted with vars in gameManager
    [SerializeField] private float xMax = 13f;
    [SerializeField] private float yMax = 4.5f;

    //[SerializeField] private float xMovement = 0;
    //[SerializeField] private float yMovement = 0;

    [SerializeField] private ProjectileController projectilePrefab;

    /*
    [SerializeField] private EngineStats engine;
    [SerializeField] private CannonStats cannon;
    */

    // https://docs.unity3d.com/Manual/ExecutionOrder.html
    
    private float deltaT = 0f;
    
    /*
    private void Start() {

        // Get engine stats
        var engineStats = engine.GetComponent<EngineStats>();
        if (engineStats != null) {
            this.acceleration = engineStats.acceleration;
            this.maxSpeed = engineStats.maxSpeed;

            // Set player health based on engine stats
            var healthManager = this.gameObject.GetComponent<PlayerHealthManager>();
            if (healthManager != null) {
                healthManager.health = engineStats.health;
            }
        }

        // Set cannon stats
        var cannonStats = cannon.GetComponent<CannonStats>();
        if (engineStats != null) {
            this.damage = cannonStats.damage;
            this.fireSpeed = cannonStats.fireSpeed;
        }

        // Update firespeed so no intial wait
        timeSinceLastShot = fireSpeed;


    }
    */

    public void Initialize(EngineStats engineStats, CannonStats cannonStats) {
        //var engineStats = engine.GetComponent<EngineStats>();
        if (engineStats != null) {
            this.acceleration = engineStats.acceleration;
            this.maxSpeed = engineStats.maxSpeed;

            // Set player health based on engine stats
            var healthManager = this.gameObject.GetComponent<PlayerHealthManager>();
            if (healthManager != null) {
                healthManager.Initialize(engineStats.health);
            }
        }

        // Set cannon stats
        //var cannonStats = cannon.GetComponent<CannonStats>();
        if (engineStats != null) {
            this.damage = cannonStats.damage;
            this.fireSpeed = cannonStats.fireSpeed;
        }

        // Update firespeed so no intial wait
        timeSinceLastShot = fireSpeed;

        this.xMax = GameManager.xMax;
        this.yMax = GameManager.yMax;

    }
    
    public void Customize(EngineStats engineStats, CannonStats cannonStats) {
        this.acceleration = engineStats.acceleration;
        this.maxSpeed = engineStats.maxSpeed;
        // Set player health based on engine stats
        var healthManager = this.gameObject.GetComponent<PlayerHealthManager>();
        if (healthManager != null) {
            healthManager.Initialize(engineStats.health);
        }

        this.damage = cannonStats.damage;
        this.fireSpeed = cannonStats.fireSpeed;
    }

    public void Update() {

        // Get change in time from last frame
        deltaT = Time.deltaTime;

        //var xMovement = 0f;
        //var yMovement = 0f;

        // https://answers.unity.com/questions/1105218/how-to-make-an-object-shoot-a-projectile.html
        // https://docs.unity3d.com/560/Documentation/Manual/Prefabs.html#:~:text=You%20can%20create%20a%20prefab,gameobject%20with%20the%20new%20one.
        // https://docs.unity3d.com/Manual/CreatingPrefabs.html
        // https://cla.purdue.edu/academic/rueffschool/ad/etb/resources/ad41700_unity3d_workshop03_f13.pdf
        // https://docs.unity3d.com/ScriptReference/Object.Instantiate.html


        aim();

        if (Input.GetKeyDown(KeyCode.M)) {
            inertiaMovement = !inertiaMovement;
        }

        if (inertiaMovement) {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                Vector3 tempMovement = movement;
                tempMovement.x += acceleration;
                if (tempMovement.magnitude < maxSpeed && transform.position.x < xMax) {
                    movement = tempMovement;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                Vector3 tempMovement = movement;
                tempMovement.x -= acceleration;
                if (tempMovement.magnitude < maxSpeed && transform.position.x > xMax * -1) {
                    movement = tempMovement;
                }
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                Vector3 tempMovement = movement;
                tempMovement.y += acceleration;
                if (tempMovement.magnitude < maxSpeed && transform.position.y < yMax) {
                    movement = tempMovement;
                }
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                Vector3 tempMovement = movement;
                tempMovement.y -= acceleration;
                if (tempMovement.magnitude < maxSpeed && transform.position.y > yMax * -1) {
                    movement = tempMovement;
                }
            }

            // Brake
            if (Input.GetKey(KeyCode.B) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) {
                if (movement.magnitude < 0.10) {
                    movement = new Vector3(0,0,0);
                } else {
                    movement = new Vector3(3f * movement.x / 4f, 3f * movement.y / 4f, 0);
                }
                
            }

        } else { // Simple movement

            movement.x = 0;
            movement.y = 0;

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                /*
                if (movement.x + acceleration <= maxSpeed) {
                    movement.x += acceleration;
                }
                */
                movement.x = maxSpeed;

            } 
            /*
            else if (movement.x > 0) { // Deccelerate while button released
                if (movement.x < 0.10) {
                    movement.x = 0;
                } else {
                    //movement = new Vector3(3f * movement.x / 4f, 3f * movement.y / 4f, 0);
                    movement.x = movement.x * 3f / 4f;
                }
            }
            */


            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                /*
                if (movement.x - acceleration >= maxSpeed * -1) {
                    movement.x -= acceleration;
                }
                */
                movement.x = maxSpeed * -1;

            }
            /*
            else if (movement.x < 0) { // Deccelerate while button released
                if (movement.x > -0.10) {
                    movement.x = 0;
                } else {
                    movement.x = movement.x * 3f / 4f;
                }
            }
            */

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                /*
                if (movement.y + acceleration <= maxSpeed) {
                    movement.y += acceleration;
                }
                */

                movement.y = maxSpeed;

            }
            /*
            else if (movement.y > 0) { // Deccelerate while button released
                if (movement.y < 0.10) {
                    movement.y = 0;
                } else {
                    movement.y = movement.y * 3f / 4f;
                }
            }
            */

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                /*
                if (movement.y - acceleration >= maxSpeed * -1) {
                    movement.y -= acceleration;
                }
                */

                movement.y = maxSpeed * -1;

            }
            /*
            else if (movement.y < 0) { // Deccelerate while button released
                if (movement.y > -0.10) {
                    movement.y = 0;
                } else {
                    movement.y = movement.y * 3f / 4f;
                }
            }
            */
        }

        // Shoot right
        /*
        if (Input.GetKeyDown(KeyCode.Space) && timeSinceLastShot > fireSpeed) {
            //GameObject bullet = Instantiate(LaserShot)
            var projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<ProjectileController>().damage = this.damage;
            //projectile.GetComponent<ProjectileController>().shotByPlayer = true;
            projectile.GetComponent<ProjectileController>().speed = maxSpeed+10f;
            projectile.transform.localPosition = transform.localPosition + new Vector3(1.25f ,0 ,0);


            // Update projectile direction
            // Update projectile speed based on player speed
            // Update projectile damage

            timeSinceLastShot = 0f;
        }
        */

        // Shoot at mouse
        if (Input.GetMouseButton(0) && timeSinceLastShot > fireSpeed)
        {
            var projectile = Instantiate(this.projectilePrefab);
            
            projectile.transform.position = gameObject.transform.position + aimDirection;
            projectile.Initialize(this.damage, this.maxSpeed + 10f, aimDirection);
            
            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += deltaT;
        

        // TODO: If out of bounds, move into bounds
        if (transform.position.x > xMax && movement.x > 0) {
            movement.x = 0;
        }
        if (transform.position.x < xMax * -1 && movement.x < 0) {
            movement.x = 0;
        }
        if (transform.position.y > yMax && movement.y > 0) {
            movement.y = 0;
        }
        if (transform.position.y < yMax * -1 && movement.y < 0) {
            movement.y = 0;
        }

        transform.Translate(movement * deltaT, Space.World);
    }

    private void aim() {

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var gamePlane = new Plane(Vector3.back, Vector3.zero);

        if (gamePlane.Raycast(mouseRay, out var distance)) {
            
            // get hit point
            var hitPoint = mouseRay.GetPoint(distance);

            aimDirection = (hitPoint - gameObject.transform.position).normalized;

            // rotate player
            //transform.LookAt(hitPoint * -1, Vector3.back);

            transform.rotation = Quaternion.LookRotation(aimDirection * -1, Vector3.back);

        }
    }

    /*
    private void OnTriggerEnter(Collider other){
        var asteroid = other.GetComponent<AsteroidController>();

        if (asteroid != null) {
            this.gameObject.GetComponent<PlayerHealthManager>().damage(asteroid.GetComponent<AsteroidController>().damage);
            Destroy(other.gameObject);
        }
    }
    */
    
}
