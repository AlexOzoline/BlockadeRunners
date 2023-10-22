// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class StarManager : MonoBehaviour
{       

    [SerializeField] private ParticleSystem stars;

    [SerializeField] private float starAcceleration = 0.5f;

    [SerializeField] private float starSpeed = 10f;

    private float time = 10f;

    public void Update() {
        // https://answers.unity.com/questions/1655687/how-can-i-change-the-speed-of-my-particles-during.html
        // https://docs.unity3d.com/ScriptReference/ParticleSystem.html

        //time += Time.deltaTime;

        time = Mathf.Sqrt(GameManager.gameTime)+10f;

        var main = stars.main;

        starSpeed = (time) * starAcceleration;

        main.startSpeedMultiplier = starSpeed * 2;
    }
    
}
