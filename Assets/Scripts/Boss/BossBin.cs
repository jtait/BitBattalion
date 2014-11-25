using UnityEngine;
using System.Collections;

public class BossBin : GenericBoss
{

    private const int BASE_HEALTH = 50; // the base health of the bin
    private const float MIN_TIME_UNTIL_INHALE = 3;
    private const float MAX_TIME_UNTIL_INHALE = 8;
    private const float MIN_INHALE_DURATION = 2.1f;
    private const float MAX_INHALE_DURATION = 4.3f;

    /* for inhale */
    private PlayerControl playerControl;
    private float nextInhale = 5;

    protected override void Start()
    {
        health = BASE_HEALTH * gParams.difficulty / 2; // multiply boss health by difficulty level
        playerControl = playerTransform.GetComponent<PlayerControl>();
        activateDistance = 10f;
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
        if (col.collider.tag == "PlayerCollider")
        {
            LoseSequence();
        }

    }

    /* inhale the player */
    IEnumerator Inhale(float duration)
    {
        playerControl.moveOverride = true;
        playerControl.movementOverrideVector = transform.position;
        yield return new WaitForSeconds(duration);
        playerControl.movementOverrideVector = Vector3.zero;
        playerControl.moveOverride = false;
    }

    /* called if the player loses the fight */
    public void LoseSequence()
    {
        playerTransform.GetComponent<PlayerControl>().PlayerDeath();
        playerControl.moveOverride = false;
    }

}
