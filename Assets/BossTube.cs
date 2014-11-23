using UnityEngine;
using System.Collections;

public class BossTube : GenericBoss {

    private const int BASE_HEALTH = 10; // the base health of the tube
    private int allowableEscapedBits; // the number of bits that can escape before the player loses
    private int escapedBits = 0;

	protected override void Start () {
        health = BASE_HEALTH * gParams.difficulty / 2;
        allowableEscapedBits = 19; // arbitrary number
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
        base.DeathCheck();
    }

    protected override void DeathSequence()
    {



        // last thing
        base.DeathSequence();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);

        if (col.collider.tag == "Enemy")
        {
            escapedBits++;
        }
    }

    
    private override void LoseSequence()
    {

    }

}
