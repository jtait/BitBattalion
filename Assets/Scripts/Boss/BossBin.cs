using UnityEngine;
using System.Collections;

public class BossBin : GenericBoss
{

    private const int BASE_HEALTH = 150; // the base health of the bin
    private const float MIN_TIME_UNTIL_INHALE = 3;
    private const float MAX_TIME_UNTIL_INHALE = 8;
    private const float MIN_INHALE_DURATION = 2.1f;
    private const float MAX_INHALE_DURATION = 4.3f;
    private const int BASE_POINTS = 4000;

    /* for inhale */
    private PlayerControl playerControl;
    private float nextInhale = 5;
    private ParticleEmitter inhaleParticles;

    protected override void Start()
    {
        points = BASE_POINTS * difficulty;
        health = BASE_HEALTH * gParams.difficulty / 2; // multiply boss health by difficulty level
        playerControl = playerTransform.GetComponent<PlayerControl>();
        inhaleParticles = GetComponentInChildren<ParticleEmitter>();
        inhaleParticles.emit = false;
        activateDistance = 10f;
        nextLevel = "Story_Level_03";
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
        Vacuum(false);
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
        Vacuum(true);
        yield return new WaitForSeconds(duration);
        Vacuum(false);
    }

    /* called if the player loses the fight */
    public override void LoseSequence()
    {
        playerTransform.GetComponent<PlayerControl>().PlayerDeath();
        Vacuum(false);
    }

    private void Vacuum(bool set){
        playerControl.moveOverride = set;
        inhaleParticles.emit = set;
        if(set){
            playerControl.movementOverrideVector = transform.position;
        }
        else{
            playerControl.movementOverrideVector = Vector3.zero;
        }
    }

}
