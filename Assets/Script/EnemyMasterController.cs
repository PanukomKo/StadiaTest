using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMasterController : MonoBehaviour {

    public GameObject monsterPrefab;
    public List<Monster> enemyList = new List<Monster>();

    bool spawning = true;

    Vector3[] spawnPosition = new[] { new Vector3(-4, 0.1f, 0), new Vector3(4, 0.1f, 0), new Vector3(0, 4, 0), new Vector3(0, -4, 0) };

    void OnEnable()
    {
        GameMasterController.OnDeath += stopAll;
    }

    void OnDisable()
    {
        GameMasterController.OnDeath -= stopAll;
    }

    public void createEnemy()
    {
        int randomDirection = Random.Range(0, 4);

        GameObject newEnemy = Instantiate(monsterPrefab, spawnPosition[randomDirection], transform.rotation, transform);
        enemyList.Add(newEnemy.GetComponent<Monster>());
    }

    public void stopAll()
    {
        spawning = false;
        StopAllCoroutines();
        foreach (Monster monster in enemyList)
        {
            monster.setStop(true);
        }
        StartCoroutine(destroyAllEnemies());
    }

    public void spawn()
    {
        StopAllCoroutines();
        spawning = true;
        StartCoroutine(startSpawnEnemies());
    }

    IEnumerator startSpawnEnemies()
    {
        while (spawning)
        {
            createEnemy();
            yield return new WaitForSeconds(0.8f);
        }
    }

    IEnumerator destroyAllEnemies()
    {
        yield return new WaitForSeconds(1.2f);
        enemyList.RemoveAll(s => s == null);
        foreach (Monster enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();
    }
}
