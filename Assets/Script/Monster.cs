using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    float moveSpeed = 70f;

    Vector3 playerPosition = new Vector3(0, 0, 0);

    public GameObject coinPrefab;
    public GameObject coinTab;

    private float startTime;
    private Animator animator;

    public bool isStopMonster = false;

    void Start()
    {
        startTime = Time.time;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isStopMonster)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, (Time.time - startTime) / moveSpeed);
        }
    }

    public void getHit()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(playHit());
    }

    public void setStop(bool stop)
    {
        isStopMonster = stop;
    }

    IEnumerator playHit()
    {
        animator.SetTrigger("Hit");
        GameMasterController.Instance.score++;
        if (Random.value > 0.7f)
        {
            Instantiate(coinPrefab, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
