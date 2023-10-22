using UnityEngine;

public class HealRingController : MonoBehaviour
{
    public float timeAtSpawn = 0f;

    // Move speed for the ring
    public float speed = 30f;
    
    // Normalized vector for direction
    [SerializeField] private Vector3 direction = Vector3.left;
    // Vector is 2D - not using Z dimension

    // Amount of health added
    [SerializeField] private float healAmount = 5f;

    [SerializeField] private GameObject explosionPreFab;

    public void Initialize(float time, float playerLane) {
        timeAtSpawn = Mathf.Sqrt(time);
        this.speed = (timeAtSpawn) * 1.5f;

        // Spawned somewhere in the player lane
        transform.SetPositionAndRotation(new Vector3(GameManager.xMax + 5f, playerLane, 0), Quaternion.identity);
    }

    // https://docs.unity3d.com/Manual/ExecutionOrder.html
    private void Update()
    {   
        transform.Translate( direction * (this.speed * Time.deltaTime), Space.World);

        
        var BoundCheck = this.gameObject.GetComponent<BoundryCheck>();
        if (BoundCheck != null) {
            BoundCheck.checkLeftBound();
        }
        

    }

    private void OnTriggerEnter(Collider other) {
        // Just get other collider, not collision information

        var playerHealthManager = other.GetComponent<PlayerHealthManager>();

        if (playerHealthManager != null) {
            playerHealthManager.heal(healAmount);
        }

        if (explosionPreFab != null) {
            var explosion = Instantiate(explosionPreFab);
            explosion.transform.position = this.transform.position;
        }

        Destroy(this.gameObject);
    }
}
