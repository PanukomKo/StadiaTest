using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverController : MonoBehaviour {

    public GameObject gameoverText;
    public Text coin;
    public Text bestScore;
    public Text score;

    public GameObject gameoverPanel;

    public GameObject optionMenu;
    public GameObject bottomMenu;

    public GameObject backButton;

    void OnEnable()
    {
        GameMasterController.OnPlay += hideGameover;
        GameMasterController.OnDeath += showGameover;
    }

    void OnDisable()
    {
        GameMasterController.OnPlay -= hideGameover;
        GameMasterController.OnDeath -= showGameover;
    }

    public void showGameover()
    {
        score.text = "Score\n" + GameMasterController.Instance.score.ToString();
        bestScore.text = "Best\n" + GameMasterController.Instance.bestScore.ToString();
        coin.text = GameMasterController.Instance.coin.ToString();
        StartCoroutine(startGameover());
    }

    public void hideGameover()
    {
        gameoverPanel.SetActive(false);
        optionMenu.SetActive(false);
        bottomMenu.SetActive(false);
        backButton.SetActive(false);
        gameoverText.SetActive(false);
    }

    IEnumerator startGameover()
    {
        yield return new WaitForSeconds(1f);
        gameoverPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        optionMenu.SetActive(true);
        bottomMenu.SetActive(true);
        backButton.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameoverText.SetActive(true);
    }
}
