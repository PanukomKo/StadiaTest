using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public GameObject tutorialPanel;
    public GameObject monsterPrefab;

    public GameObject hand;
    public GameObject indicatorBig;
    public GameObject indicatorLine;
    public GameObject indicatorEnd;
    
    private Animator animator;

    bool tutorialEnd = true;

    public delegate void TutorialAction();
    public static event TutorialAction OnTutorialEnd;

    void Start()
    { 
        animator = tutorialPanel.GetComponent<Animator>();
    }

    void OnEnable()
    {
        GameMasterController.OnPlay += startTutorial;
        SwipeController.OnSwipe += finishTutorial;
    }

    void OnDisable()
    {
        GameMasterController.OnPlay -= startTutorial;
        SwipeController.OnSwipe -= finishTutorial;
    }

    public void startTutorial()
    {
        GameObject tutorialMonster = Instantiate(monsterPrefab, new Vector3(-1.13f, 0, 0), transform.rotation, tutorialPanel.transform);
        tutorialMonster.GetComponent<Monster>().setStop(true);

        animator.enabled = true;

        hand.SetActive(false);
        indicatorBig.SetActive(false);
        indicatorLine.SetActive(false);
        indicatorEnd.SetActive(false);

        tutorialPanel.SetActive(true);
        StartCoroutine(startTutorialAnimation());
    }

    public void finishTutorial(int direction)
    {
        if (!tutorialEnd)
        {
            if (direction == 0)
            {
                StartCoroutine(stopTutorialAnimation());
                tutorialEnd = true;
            }
        }
    }

    IEnumerator startTutorialAnimation()
    {
        tutorialEnd = false;
        yield return new WaitForSeconds(0.2f);
        animator.SetTrigger("Tutorial");
    }

    IEnumerator stopTutorialAnimation()
    {
        animator.enabled = false;
        yield return new WaitForSeconds(0.5f);
        tutorialPanel.SetActive(false);
        OnTutorialEnd();
    }
}
