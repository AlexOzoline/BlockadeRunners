using UnityEngine;

public class BoundryCheck : MonoBehaviour
{

    [SerializeField] private float xMax = 13f;
    [SerializeField] private float yMax = 4.5f;

    private void Start() {
        this.xMax = GameManager.xMax;
        this.yMax = GameManager.yMax;
    }

    public void checkBound()
    {   
        

        // Check game boundaries
        if (transform.position.x > xMax + 5) {
            //print(this.gameObject + " Out of bounds");
            Destroy(this.gameObject);
        }
        if (transform.position.x < (xMax + 5) * -1) {
            //print(this.gameObject + " Out of bounds");
            Destroy(this.gameObject);
        }
        if (transform.position.y > yMax + 5) {
            //print(this.gameObject + " Out of bounds");
            Destroy(this.gameObject);
        }
        if (transform.position.y < (yMax + 5) * -1) {
            //print(this.gameObject + " Out of bounds");
            Destroy(this.gameObject);
        }
    }

    // Used for enemies and asteroids spawned offscreen, moving left
    public void checkLeftBound()
    {   
        // Check game boundaries
        if (transform.position.x < (xMax + 5) * -1) {
            //print(this.gameObject + " Out of left bound");
            Destroy(this.gameObject);
        }
        /*
        if (transform.localPosition.y > 10) {
            Destroy(this.gameObject);
        }
        if (transform.localPosition.y < -10) {
            Destroy(this.gameObject);
        }
        */
    }
    public void checkLeftBoundPlanet(float scale)
    {   
        // Check game boundaries
        if (transform.position.x < (xMax + (2f * scale) + 0.5f) * -1) {
            //print(this.gameObject + " Out of left bound");
            Destroy(this.gameObject);
        }
    }
}
