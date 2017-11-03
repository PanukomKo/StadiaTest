using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    float moveSpeed = 20f;
    public GameObject coinTab;

    public GameObject coin;
    public GameObject starParticle;

    private float startTime;
    Vector3 coinTabPosition;

    bool isGet = false;

    public delegate void CoinAction();
    public static event CoinAction OnCoin;

    void Start () {
        startTime = Time.time;
        coinTabPosition = new Vector3(2.16f, 3.55f, 0);
    }
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, coinTabPosition, (Time.time - startTime) / moveSpeed);
        if(transform.position.x >= coinTabPosition.x - (coinTab.transform.localScale.x/2f) && transform.position.y >= coinTabPosition.y - (coinTab.transform.localScale.y / 2f))
        {
            if (!isGet)
            {
                isGet = true;
                StartCoroutine(playCharChing());
            }
        }
    }

    IEnumerator playCharChing()
    {
        OnCoin();
        coin.SetActive(false);
        starParticle.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
