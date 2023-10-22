using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(float health) {
        slider.value = health;
    }
}
