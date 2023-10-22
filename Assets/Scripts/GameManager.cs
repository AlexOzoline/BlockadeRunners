// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{       
    
    // Used for menu
    [SerializeField] GameObject gameoverMenu;
    public Text scoreText;
    public Text currentScoreText;
    public Text distanceText;
    public Text cannonText;
    public Text engineText;
    private float roundedGameTime;

    //public static GameManager Instance;

    public static int score = 0;

    // Used instead of fixedTime
    // Updated every frame to keep time and score
    // Paused during boss fights - must defeat boss to progress further
    public static float gameTime = 0f;

    private bool inBossFight = false;

    // Used to create a lane for the player to fly in
    // Avoid spawning asteroids in the player lane, to ensure that player path is never completely blocked
    [SerializeField] private float playerLane;

    [SerializeField] private float timeBetweenBosses = 60f;

    // Make sure matches vars in PlayerController and BossController
    public static float xMax = 13f;
    public static float yMax = 4.5f;

    public static float laneWidth = 2;

    // OBSTACLE PREFABS
    [SerializeField] private SmallEnemyController basicEnemyPrefab;
    [SerializeField] private AsteroidController asteroidPrefab;
    [SerializeField] private BossController bossPrefab;
    private BossController currBoss;

    //BACKGROUND PREFABS
    [SerializeField] private PlanetController planetPrefab;

    [SerializeField] private ShieldRingController shieldRingPrefab;

    [SerializeField] private HealRingController healRingPrefab;

    // PLAYER VARIABLES
    [SerializeField] private PlayerController playerPreFab;

    private PlayerController player;

    [SerializeField] private EngineStats engine;
    [SerializeField] private CannonStats cannon;

    // Scene Camera
    [SerializeField] private Camera cam;

    // Additional timer variables
    private float bossFightTimer = 0f;
    private float scoreIncrimentTimer = 0f;

    private int bossNum = 0;

    /*
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Multiple GameManagers");
            Destroy(gameObject);
            return;
        }
    }
    */

    public void Awake() {
        gameTime = 0f;
        bossFightTimer = 0f;
        scoreIncrimentTimer = 0f;
        bossNum = 0;

        // Default
        cannonText.text = "Cannon: Standard";
        engineText.text = "Engine: Standard";

        // Calculate game bounds based on screen size
        // May use Camera.CalculateFrustumCorners, or Camera.orthographicSize?
        Vector3[] frustumCorners = new Vector3[4];
        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 10f, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
        print("xMax = " + frustumCorners[0].x);
        
        xMax = Mathf.Abs(frustumCorners[0].x);
        yMax = Mathf.Abs(frustumCorners[0].y);


        player = Instantiate(this.playerPreFab);
        player.Initialize(engine, cannon);
        if (Customization.cannonsFlag != -1) {
            CustomizeSpaceShip();
        } 
        player.transform.position = new Vector3(-3 * xMax / 4,0,0);

        laneWidth = yMax * 2 / 5;


        
        // Test Asteroid
        /*
        var asteroid = Instantiate(asteroidPrefab);
        asteroid.Initialize(gameTime, playerLane);
        */

        // Start coroutines for obstacles
        StartCoroutine(AsteroidCoroutine());
        StartCoroutine(BasicEnemyCoroutine());
        StartCoroutine(BonusRingCoroutine());

        //StartCoroutine(DistScoreIncriments());

        StartCoroutine(planetCoroutine());

        //StartCoroutine(BossCoroutine());

        score = 0;

        
    }

    private void CustomizeSpaceShip() {
        if (GameObject.Find("cannonsObject")) {
            GameObject cannonsObj = GameObject.Find("cannonsObject");
            cannonsObj.AddComponent<CannonStats>();

            if (Customization.cannonsFlag == 0) {
                // Heavy
                cannonsObj.GetComponent<CannonStats>().damage = 10f;
                cannonsObj.GetComponent<CannonStats>().fireSpeed = 2f;
                cannonText.text = "Cannon: Heavy";
            } else if (Customization.cannonsFlag == 1) {
                // Quick
                cannonsObj.GetComponent<CannonStats>().damage = 2.5f;
                cannonsObj.GetComponent<CannonStats>().fireSpeed = 0.5f;
                cannonText.text = "Cannon: Light";
            } else if (Customization.cannonsFlag == 2) {
                // Standard
                cannonsObj.GetComponent<CannonStats>().damage = 5f;
                cannonsObj.GetComponent<CannonStats>().fireSpeed = 1f;
                cannonText.text = "Cannon: Standard";
            }
            GameObject enginesObj = GameObject.Find("enginesObject");
            enginesObj.AddComponent<EngineStats>();
            if (Customization.enginesFlag == 0) {
                // Heavy
                enginesObj.GetComponent<EngineStats>().acceleration = 0.25f;
                enginesObj.GetComponent<EngineStats>().maxSpeed = 7f;
                enginesObj.GetComponent<EngineStats>().health = 30f;
                engineText.text = "Engine: Heavy";
            } else if (Customization.enginesFlag == 1) {
                // Light
                enginesObj.GetComponent<EngineStats>().acceleration = 0.75f;
                enginesObj.GetComponent<EngineStats>().maxSpeed = 20f;
                enginesObj.GetComponent<EngineStats>().health = 10f;
                engineText.text = "Engine: Light";
            } else if (Customization.enginesFlag == 2) {
                // Standard
                enginesObj.GetComponent<EngineStats>().acceleration = 0.5f;
                enginesObj.GetComponent<EngineStats>().maxSpeed = 15f;
                enginesObj.GetComponent<EngineStats>().health = 20f;
                engineText.text = "Engine: Standard";
            }

            player.Customize(enginesObj.GetComponent<EngineStats>(), cannonsObj.GetComponent<CannonStats>());
        }
    }

    public void Update() {
        float deltaT = Time.deltaTime;

        if (Customization.confirmPress == 1) {
            CustomizeSpaceShip();
            Customization.confirmPress = 0;
        }
        currentScoreText.text = score.ToString();
        roundedGameTime = Mathf.Round(gameTime * 100.00f) * 0.01f;
        distanceText.text = roundedGameTime.ToString();

        // If not in boss fight, update game time
        if (!inBossFight) {
            gameTime += deltaT;
            bossFightTimer += deltaT;
            scoreIncrimentTimer += deltaT;

        } else { // inBossFight
            if (currBoss == null) {
                print("Boss Defeated!");
                    
                StartCoroutine(AsteroidCoroutine());
                StartCoroutine(BasicEnemyCoroutine());

                inBossFight = false;
            }
        }

        // When spawning in enemies and obstacles, remeber to call intitialize and give gameTime (and player lane for asteroids)

        // When spawning in asteroids, reset local position to stay out of within [.5 to 1] of player lane - otherwise will just be randomized
        // ^ Handled by intialize method for asteroids, just need to pass in the player lane

        // Randomly spawn in enemies and asteroids using max and min time between, which shrink based on 1/Mathf.sqrt(gameTime);

        // Every [60] seconds or so, switch to inBossFight, and pause other stuff
        // Boss fight will continue until boss object destroyed

        // New structure for boss fights - not using coroutine
        if (bossFightTimer >= timeBetweenBosses) {
            print("Boss Fight!");

            StopCoroutine(BasicEnemyCoroutine());
            StopCoroutine(AsteroidCoroutine());

            currBoss = Instantiate(bossPrefab);
            currBoss.Initialize(gameTime, player, bossNum++);

            inBossFight = true;

            bossFightTimer = 0f;

        }

        if (scoreIncrimentTimer >= 1) {
            score++;
            scoreIncrimentTimer = 0;
        }

        // Randomly adjust playerLane - keep within bounds
        // Player lane has a width of 2, denoted by the center point of the lane
        if (playerLane > yMax-1) {
            playerLane += Random.Range(-0.5f, 0f);
        } else if (playerLane < -1*yMax + 1) {
            playerLane += Random.Range(0, 0.5f);
        } else {
            playerLane += Random.Range(-0.5f, 0.5f);
        }

        // Check if player destroyed
        // If so, wait a second or two for destruction animation, then move to game over screen (pass gameTime to use as score)
        if (player == null) {
            // Move to game over screen
            StartCoroutine(GameOver());
            print("Game Over. Score = " + score);
        }
    }

    public PlayerController getPlayer() {
        return player;
    }

    IEnumerator AsteroidCoroutine() {
        while (true) {
            //if (!inBossFight) {
                // Fiddle around with this line to change the rate at which asteroids spawn
                yield return new WaitForSeconds(Random.Range(10/Mathf.Sqrt(gameTime+10f), 25/Mathf.Sqrt(gameTime+10)));
                //yield return new WaitForSeconds(Random.Range(3f, 5f));

                // launch asteroid

                //print("Spawn asteroid");
                var asteroid = Instantiate(asteroidPrefab);
                asteroid.Initialize(gameTime, playerLane);
            //}
        }
    }

    IEnumerator BasicEnemyCoroutine() {
        while (true) {
            //if (!inBossFight) {
                yield return new WaitForSeconds(Random.Range(30/Mathf.Sqrt(gameTime+10), 60/Mathf.Sqrt(gameTime+10)));

                // launch small enemy

                //print("Spawn basic enemy");
                var enemy = Instantiate(basicEnemyPrefab);
                enemy.Initialize(gameTime);
            //}
        }
    }

    IEnumerator BonusRingCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(60/Mathf.Sqrt(gameTime+10), 90/Mathf.Sqrt(gameTime+10)));

            // launch shield ring

            print("Spawn Shield Ring");
            var ring = Instantiate(shieldRingPrefab);
            ring.Initialize(gameTime, playerLane);

            // Will alternate b/t health and shields
            yield return new WaitForSeconds(Random.Range(60/Mathf.Sqrt(gameTime+10), 90/Mathf.Sqrt(gameTime+10)));

            // launch health ring

            print("Spawn Health Ring");
            var hRing = Instantiate(healRingPrefab);
            hRing.Initialize(gameTime, playerLane);
        }
    }

    IEnumerator planetCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(10, 20));

            // launch planet

            //print("Spawn Planet");
            var planet = Instantiate(planetPrefab);
            planet.Initialize(gameTime);
        }
    }

    IEnumerator GameOver() {
        while (true) {
            
            yield return new WaitForSeconds(2f);

            Time.timeScale = 0f;
            gameoverMenu.SetActive(true);
            scoreText.text = score.ToString();
        
        }
    }


    // COROUTINE GRAVEYARD
    /*

    IEnumerator DistScoreIncriments() {
        while (true) {
            yield return new WaitForSeconds(1f);
                score++;
            
        }
    }

    IEnumerator BossCoroutine() {
        while (true) {
            
            yield return new WaitForSeconds(60f);

            print("Boss Fight!");

            StopCoroutine(BasicEnemyCoroutine());
            StopCoroutine(AsteroidCoroutine());

            currBoss = Instantiate(bossPrefab);
            currBoss.Initialize(gameTime, player);

            inBossFight = true;

            while(inBossFight) {
                if (currBoss == null) {
                    print("Boss Defeated!");
                    
                    StartCoroutine(AsteroidCoroutine());
                    StartCoroutine(BasicEnemyCoroutine());

                    inBossFight = false;
                }
            }

        }
    }
    */
}

