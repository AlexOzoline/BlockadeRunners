using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject instructionsPanel, customizationPanel, spaceship;

    public void newGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void howToPlay()
    {
        instructionsPanel.SetActive(true);
        spaceship.SetActive(false);
    }

    public void customization()
    {
        customizationPanel.SetActive(true);

        // CannonsDropDown: set default to standard
        if (GameObject.Find("cannonsObject") && Customization.cannonsFlag != -1) {
            GameObject.Find("CustomizationPanel").transform.GetChild(8).GetComponent<Customization>().cannonsDropdown.value = Customization.cannonsFlag;
        } else {
            GameObject.Find("CustomizationPanel").transform.GetChild(8).GetComponent<Customization>().cannonsDropdown.value = 2;
        }

        // EnginesDropDown: set default to standard
        if (GameObject.Find("enginesObject") && Customization.enginesFlag != -1) {
            GameObject.Find("CustomizationPanel").transform.GetChild(9).GetComponent<Customization>().enginesDropdown.value = Customization.enginesFlag;
        } else {
            GameObject.Find("CustomizationPanel").transform.GetChild(9).GetComponent<Customization>().enginesDropdown.value = 2;
        }

        spaceship.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();  
    }

    public void backFromInstructions()
    {
        instructionsPanel.SetActive(false);
        spaceship.SetActive(true);
    }

    public void backFromCustomization()
    {
        customizationPanel.SetActive(false);
        spaceship.SetActive(true);
    }

}
