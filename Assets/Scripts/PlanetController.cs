using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script that randomizes the scale of given object when the object is generated
public class PlanetController : MonoBehaviour
{
    public float globalScaleMultiplier = 1f;
    public float scaleMin = 0.7f;
    public float scaleMax = 3f;
    public float speed = 1f;
    private Vector3 direction = new Vector3(-1, 0, 0);
    private float planetScale = 0;
    // Texture settings
    public int width = 144;
    public int height = 144;
    public float perlinScale = 15;
    public Color32 colour1 = new Color(0,0,0,0);
    public Color32 colour2 = new Color(0,0,0,0);

    public void Initialize(float time)
    {   
        //Randomly Generate a scale for a new procedurally generated object
        Vector3 newScale = Vector3.one;
        float scale = Random.Range(scaleMin, scaleMax);
        this.planetScale = scale;
        newScale = new Vector3(scale, scale, 0);
        transform.localScale = newScale * globalScaleMultiplier;
        // set the colours we are going to use for the planets
        setPalette();
        // vary the speed based on game time.
        float timeAtSpawn = Mathf.Sqrt(time + 10);
        this.speed = timeAtSpawn * 0.45f;

        // get the renderer and create a new texture from scratch
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = PerlinNoise();

        // randomise the colour
        //Color newColour = Random.ColorHSV(0f, .25f, 0.4f, 1f);
        //this.GetComponent<Renderer>().material.color = newColour;

        // place object
        var yPos = Random.Range(-1 * GameManager.yMax, GameManager.yMax);
        transform.position = new Vector3(GameManager.xMax + 2f * scale + 5, yPos, 6);
    }

    private void Update()
    {   
        // slowly move each frame
        transform.Translate( direction * (speed * Time.deltaTime), Space.World);
        // utilize Boundry checking to destroy object when it is off screen
        this.gameObject.GetComponent<BoundryCheck>().checkLeftBoundPlanet(this.planetScale);
    }

    private Texture2D PerlinNoise() 
    { 
        Texture2D texture = new Texture2D(width, height);
        int perlinSeed = Random.Range(1,100000);
        // replace each pixel with a generated output by a perlin noise algorithm
        // using a scale variable to "zoom in" on the texture to enhance pattern visibility
        for ( int i = 0; i < width; i++) 
        {   
            for(int j =0; j <height; j++)
            {   
                float normalised_i = ((float)i + perlinSeed) / width * perlinScale;
                float normalised_j = ((float)j + perlinSeed)/ height * perlinScale;
                float output = Mathf.PerlinNoise(normalised_i, normalised_j);

                if(output < 0.5f) 
                { 
                    texture.SetPixel(i, j, colour1); 
                }
                else 
                {
                    texture.SetPixel(i, j, colour2); 
                }
                //Color colour = new Color (output, output, output);
                //texture.SetPixel(i, j, colour);
            }
        }
        // apply the texture after work is done
        texture.Apply();
        return texture;
    }
    private void setPalette() 
    {   
        // Large array full of colour pairs
        Color32[] palettes = {new Color32(148,91,80,255), new Color32(109,123,150,255), new Color32(122,115,157,255), new Color32(78,72,106,255),new Color32(32,99,172,255), new Color32(1,66,135,255) 
        ,new Color32(233,146,148,255), new Color32(111,41,34,255),new Color32(93,175,98,255), new Color32(40,94,47,255),new Color32(60,160,176,255), new Color32(11,127,136,255)
        ,new Color32(58,16,16,255), new Color32(49,30,26,255), new Color32(76,109,96,255), new Color32(120,137,126,255), new Color32(159,125,54,255), new Color32(120,90,26,255)
        ,new Color32(0,104,99,255), new Color32(2,148,139,255), new Color32(135,83,136,255), new Color32(143,102,142,255), new Color32(17,55,24,255), new Color32(15,29,14,255)
        ,new Color32(100,71,55,255), new Color32(130,87,62,255),new Color32(62,124,90,255), new Color32(97,84,84,255)};
        // Pick a random colour pair
        float randFloat = Random.Range(0, 8);
        int randInt = (int)Mathf.Floor(randFloat);
        float colourOrder = Random.Range(0, 1);
        // randomly decide which colour will be the primary and which is the secondary.
        if(colourOrder < 0.5) 
        {
            this.colour1 = palettes[2 * randInt];
            this.colour2 = palettes[2 * randInt + 1];
        }
        else
        {
            this.colour2 = palettes[2 * randInt];
            this.colour1 = palettes[2 * randInt + 1];
        }
        return;
    }
}