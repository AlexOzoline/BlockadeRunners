using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customization : MonoBehaviour
{
    public Text cannonsText2, enginesText2;
    public Dropdown cannonsDropdown, enginesDropdown;

    public static int cannonsFlag = -1;
    public static int enginesFlag = -1;
    public static int confirmPress = 0;
    public HealthBar healthBar;

    
    [SerializeField] GameObject heavyCannonsPrefab, quickCannonsPrefab, standardCannonsPrefab, heavyEnginesPrefab, lightEnginesPrefab, standardEnginesPrefab; /* cannonsObj, enginesObj,*/
    [SerializeField] GameObject heavyCannonModel, lightCannonModel, standardCannonModel;
    [SerializeField] GameObject heavyEngineModel, lightEngineModel, standardEngineModel;
    

    public void cannonsSelector()
    {
        if (cannonsDropdown.value == 0)
        {
            // Heavy Cannon
            cannonsText2.text = "Long cooldown\nHigh damage";
            cannonsFlag = 0;
            heavyCannonModel.SetActive(true);
            lightCannonModel.SetActive(false);
            standardCannonModel.SetActive(false);
            // cannonsDropdown.RefreshShownValue();
        }
        if (cannonsDropdown.value == 1)
        {
            // Light Cannon
            cannonsText2.text = "Short cooldown\nLow damage";
            cannonsFlag = 1;
            heavyCannonModel.SetActive(false);
            lightCannonModel.SetActive(true);
            standardCannonModel.SetActive(false);
            // cannonsDropdown.RefreshShownValue();
        }
        if (cannonsDropdown.value == 2)
        {
            // Standard Cannon
            cannonsText2.text = "Average stats";
            cannonsFlag = 2;
            heavyCannonModel.SetActive(false);
            lightCannonModel.SetActive(false);
            standardCannonModel.SetActive(true);
            // cannonsDropdown.RefreshShownValue();
        }
    }

    public void enginesSelector()
    {
        if (enginesDropdown.value == 0)
        {
            // Heavy Engine
            enginesText2.text = "Slow maneuvering\nHigh ship health";
            enginesFlag = 0;
            heavyEngineModel.SetActive(true);
            lightEngineModel.SetActive(false);
            standardEngineModel.SetActive(false);
            healthBar.SetHealth(30f);
            // cannonsDropdown.RefreshShownValue();
        }
        if (enginesDropdown.value == 1)
        {
            // Light Engine
            enginesText2.text = "Quick maneuvering\nLow ship health";
            enginesFlag = 1;
            heavyEngineModel.SetActive(false);
            lightEngineModel.SetActive(true);
            standardEngineModel.SetActive(false);
            healthBar.SetHealth(10f);
            // cannonsDropdown.RefreshShownValue();
        }
        if (enginesDropdown.value == 2)
        {
            // Standard Engine
            enginesText2.text = "Average stats";
            enginesFlag = 2;
            heavyEngineModel.SetActive(false);
            lightEngineModel.SetActive(false);
            standardEngineModel.SetActive(true);
            healthBar.SetHealth(20f);
            // cannonsDropdown.RefreshShownValue();
        }
    }

    public void confirmSelection()
    {
        confirmPress = 1;
        if (cannonsFlag == 0)
        {
            Debug.Log("cannon0");
        }
        if (cannonsFlag == 1)
        {
            Debug.Log("cannon1");
            // cannonsObj = Instantiate(quickCannonsPrefab);
        }
        if (cannonsFlag == 2)
        {
            Debug.Log("cannon2");
            // cannonsObj = Instantiate(standardCannonsPrefab);
        }
        if (enginesFlag == 0)
        {
            Debug.Log("engine0");
            // enginesObj = Instantiate(heavyEnginesPrefab);
        }
        if (enginesFlag == 1)
        {
            Debug.Log("engine1");
            // enginesObj = Instantiate(lightEnginesPrefab);
        }
        if (enginesFlag == 2)
        {
            Debug.Log("engine2");
            // enginesObj = Instantiate(standardEnginesPrefab);
        }
    }

    void update(){
        
        // cannonsDropdown.RefreshShownValue();
    }

    public GameObject heavyCannons() {
        return heavyCannonsPrefab;
    }
}
