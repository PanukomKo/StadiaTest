using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

    public float power = 0.1f;
    public float duration = 1f;
    public float slowDownAmount = 1.0f;

    public GameObject damageBackDrop;

    Vector3 startPosition;

    float initialDuration;
    float shakeDuration = 0.5f;

    void Start () {
        startPosition = transform.localPosition;
        initialDuration = duration;
    }

    void OnEnable()
    {
        GameMasterController.OnDamage += shake;
    }

    void OnDisable()
    {
        GameMasterController.OnDamage -= shake;
    }

    public void shake()
    {
        StopAllCoroutines();
        StartCoroutine(startShake());
    }

    IEnumerator startShake()
    {
        float _startTime = Time.time;
        float _endTime = _startTime + shakeDuration;
        duration = initialDuration;

        damageBackDrop.SetActive(true);

        while (Time.time < _endTime)
        {
            if (duration > 0)
            {
                transform.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            yield return null;
        }

        transform.localPosition = startPosition;
        damageBackDrop.SetActive(false);
    }
}
