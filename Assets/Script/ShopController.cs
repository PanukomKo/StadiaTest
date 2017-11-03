using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour {

    public GameObject shopPanel;

    public GameObject characterSelected;
    public GameObject characterRight;
    public GameObject characterLeft;

    public GameObject optionMenu;
    public GameObject upgradeButton;
    public GameObject selectButton;
    public GameObject gahcaButton;

    public GameObject backButton;

    public void showShop()
    {
        StartCoroutine(startGameover());
    }

    public void hideShop()
    {
        shopPanel.SetActive(false);
        optionMenu.SetActive(false);
        gahcaButton.SetActive(false);
        backButton.SetActive(false);
    }

    IEnumerator startGameover()
    {
        yield return new WaitForSeconds(0.2f);
        shopPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        optionMenu.SetActive(true);
        gahcaButton.SetActive(true);
        backButton.SetActive(true);
    }
}
