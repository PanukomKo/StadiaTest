using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterController : MonoBehaviour {

    public int coin = 0;
    public int score = 0;
    public int bestScore = 0;
    public int life = 3;

    public GameObject lifePrefab;
    public GameObject lifeTab;
    private List<GameObject> lifePool = new List<GameObject>();

    public GameObject coinTab;

    public GameObject logo;
    public GameObject optionMenu;
    public GameObject bottomMenu;

    public GameObject gameoverPanel;
    public GameObject gameoverText;

    public GameObject upgradePanel;

    public GameObject shopPanel;

    public GameObject backButton;
    public GameObject gachaButton;

    public GameObject enemiesHorde;

    private Animator coinTabAnimator;
    private Animator bottomMenuAnimator;
    private Animator optionMenuAnimator;
    bool isOptionExpanded = false;

    private Animator playerAnimator;
    string[] attackAnimation = { "AttackLeft", "AttackRight", "AttackUp", "AttackDown" };

    bool isAttacking = false;
    float lifeObjectOffset = 0.8f;

    public delegate void PlayAction();
    public static event PlayAction OnPlay;

    public delegate void DamageAction();
    public static event DamageAction OnDamage;

    public delegate void DeathAction();
    public static event DeathAction OnDeath;

    private static GameMasterController instance = null;
    public static GameMasterController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameMasterController>();
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        optionMenuAnimator = optionMenu.GetComponent<Animator>();
        bottomMenuAnimator = bottomMenu.GetComponent<Animator>();
        coinTabAnimator = coinTab.GetComponent<Animator>();
    }

    void OnEnable()
    {
        SwipeController.OnSwipe += attack;
        TutorialController.OnTutorialEnd += startSpawn;
        Coin.OnCoin += bounce;
    }

    void OnDisable()
    {
        SwipeController.OnSwipe -= attack;
        TutorialController.OnTutorialEnd -= startSpawn;
        Coin.OnCoin -= bounce;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (isAttacking)
        {
            coll.gameObject.GetComponent<Monster>().getHit();
        }
        else
        {
            OnDamage();
            StartCoroutine(decreaseLife());
            Destroy(coll.gameObject);
        }
    }

    public void playGame()
    {
        life = 3;
        score = 0;
        coin = 0;
        StartCoroutine(pressPlay());
    }

    public void showUpgrade()
    {
        gameoverPanel.SetActive(false);
        gameoverText.SetActive(false);
        StartCoroutine(pressUpgrade());
    }

    public void showShop()
    {
        gameoverPanel.SetActive(false);
        gameoverText.SetActive(false);
        StartCoroutine(pressShop());
    }

    public void attack(int _direction)
    {
        StartCoroutine(playAttack(_direction));
    }

    public void createLifeObject()
    {
        lifePool.RemoveAll(s => s == null);
        foreach (GameObject life in lifePool)
        {
            Destroy(life);
        }
        lifePool.Clear();

        for (int i=0; i<life; i++)
        {
            GameObject newLife = Instantiate(lifePrefab, transform.position, transform.rotation, lifeTab.transform);
            newLife.transform.localPosition = new Vector3((i * lifeObjectOffset), 0, 0);
            lifePool.Add(newLife);
        }
    }

    public void clickOptionMenu()
    {
        if (!isOptionExpanded)
        {
            optionMenuAnimator.SetTrigger("Expand");
            isOptionExpanded = true;
        }
        else
        {
            optionMenuAnimator.SetTrigger("Collapse");
            isOptionExpanded = false;
        }
    }

    public void bounce()
    {
        coinTabAnimator.SetTrigger("Bounce");
        coin++;
    }

    public void startSpawn()
    {
        createLifeObject();
        lifeTab.SetActive(true);
        coinTab.SetActive(true);
        enemiesHorde.SetActive(true);
        enemiesHorde.GetComponent<EnemyMasterController>().spawn();
    }

    public void back()
    {
        bottomMenu.SetActive(true);
        logo.SetActive(true);
        optionMenu.SetActive(true);

        gameoverPanel.SetActive(false);
        gameoverText.SetActive(false);

        upgradePanel.SetActive(false);

        shopPanel.SetActive(false);

        backButton.SetActive(false);
        gachaButton.SetActive(false);
    }

    IEnumerator decreaseLife()
    {
        int lifePosition = life - 1;
        life--;

        lifePool[lifePosition].GetComponent<Animator>().SetTrigger("Decrease");

        if(life <= 0)
        {
            if (bestScore < score)
            {
                bestScore = score;
            }

            OnDeath();
            yield return new WaitForSeconds(1);
            coinTab.SetActive(false);
        }

        yield return new WaitForSeconds(1.5f);

        lifePool[lifePosition].SetActive(false);
    }

    IEnumerator playAttack(int _direction)
    {
        isAttacking = true;
        playerAnimator.SetTrigger(attackAnimation[_direction]);
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    IEnumerator playOptionExpand()
    {
        optionMenuAnimator.SetTrigger("Expand");
        yield return new WaitForSeconds(0.3f);
        isOptionExpanded = true;
    }

    IEnumerator playOptionCollapse()
    {
        optionMenuAnimator.SetTrigger("Collapse");
        yield return new WaitForSeconds(0.3f);
        isOptionExpanded = false;
    }

    IEnumerator pressPlay()
    {
        bottomMenuAnimator.SetTrigger("Play");
        yield return new WaitForSeconds(0.1f);

        bottomMenu.SetActive(false);
        logo.SetActive(false);
        optionMenu.SetActive(false);

        OnPlay();
    }

    IEnumerator pressUpgrade()
    {
        bottomMenuAnimator.SetTrigger("Upgrade");
        yield return new WaitForSeconds(0.1f);
        bottomMenu.SetActive(false);
        logo.SetActive(false);
        upgradePanel.SetActive(true);
        yield return new WaitForSeconds(1);
        backButton.SetActive(true);
    }

    IEnumerator pressShop()
    {
        bottomMenuAnimator.SetTrigger("Shop");
        yield return new WaitForSeconds(0.1f);
        bottomMenu.SetActive(false);
        logo.SetActive(false);
        shopPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        backButton.SetActive(true);
        gachaButton.SetActive(true);
    }
}
