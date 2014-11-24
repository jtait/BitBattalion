using UnityEngine;
using System.Collections;

public class BossBin : GenericBoss
{

    private const int BASE_HEALTH = 50; // the base health of the bin
    private const float MIN_TIME_UNTIL_INHALE = 3;
    private const float MAX_TIME_UNTIL_INHALE = 8;
    private const float MIN_INHALE_DURATION = 2.1f;
    private const float MAX_INHALE_DURATION = 4.3f;
    private const float ACTIVATE_DISTANCE = 10f; // the proximity of the player before becoming active

    /* for inhale */
    private PlayerControl playerControl;
    private float nextInhale = 5;

    protected override void Start()
    {
        health = BASE_HEALTH * gParams.difficulty / 2; // multiply boss health by difficulty level
        playerControl = playerTransform.GetComponentInChildren<PlayerControl>();
    }

    void FixedUpdate()
    {
        if (Time.time > nextInhale && bossActive)
        {
            StartCoroutine(Inhale(Random.Range(MAX_INHALE_DURATION, MAX_INHALE_DURATION)));
            nextInhale = Time.time + Random.Range(MIN_TIME_UNTIL_INHALE, MAX_TIME_UNTIL_INHALE);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (playerTransform.position.y > transform.position.y - ACTIVATE_DISTANCE)
            bossActive = true;
    }

    protected override void DeathCheck()
    {
        base.DeathCheck(); // checks if health <= 0
    }

    protected override void DeathSequence()
    {
        base.DeathSequence();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col); // detect player weapon hits

        /* if the player touches the boss, the player loses */
        if (col.collider.tag == "Player")
        {
            LoseSequence();
        }

    }

    protected override void LoseSequence()
    {
        base.LoseSequence();
    }

    /* inhale the player */
    IEnumerator Inhale(float duration)
    {
        playerControl.moveOverride = true;
        playerControl.movementOverrideVector = transform.position;
        yield return new WaitForSeconds(duration);
        playerControl.movementOverrideVector = transform.position;
        playerControl.moveOverride = false;
    }

}
