using UnityEngine;
using System.Collections;

public class BossTube : GenericBoss {

    private const int BASE_HEALTH = 10; // the base health of the tube
    private int allowableEscapedBits; // the number of bits that can escape before the player loses
    private int escapedBits = 0; // keeps track of the number of escaped bits

	protected override void Start () {
        health = BASE_HEALTH * gParams.difficulty / 2; // multiply boss health by difficulty level
        allowableEscapedBits = 19 * (1 / gParams.difficulty); // arbitrary number
	}

    protected override void Update()
    {
        base.Update();
        if (escapedBits > allowableEscapedBits)
        {
            LoseSequence();
        }
    }

    protected override void DeathCheck()
    {
        base.DeathCheck(); // checks if health <= 0
    }

    protected override void DeathSequence()
    {
        base.DeathSequence();
        renderer.active = false;
        collider.enabled = false;
        base.DestroyAllEnemiesAndSpawners();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col); // detect player weapon hits

        if (col.collider.tag == "Enemy")
        {
            escapedBits++;
            Destroy(col.gameObject); // destroy the bit
        }
    }

    
    protected override void LoseSequence()
    {
        playerTransform.GetComponent<PlayerControl>().PlayerDeath();
    }

}
