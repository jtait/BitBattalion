using UnityEngine;
using System.Collections;

public abstract class GenericBoss : MonoBehaviour {

    protected int health;
    protected GameParameters gParams;
    protected const float WAIT_TIME_FOR_LEVEL_LOAD = 4;

    void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
    }

	protected virtual void Start () {
	
	}
	
	protected virtual void Update () {
        DeathCheck();
	}

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Weapon")
        {
            health--;
        }
    }

    protected virtual void DeathCheck()
    {
        if (health <= 0)
        {
            DeathSequence();
        }
    }

    /* called if the player wins and the boss dies */
    protected virtual void DeathSequence()
    {
        /* override in child */

        /* call base at the end of the death sequence */
        StartCoroutine(WaitForLevelLoad(WAIT_TIME_FOR_LEVEL_LOAD));  
    }

    protected IEnumerator WaitForLevelLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // load next level here
    }

    /* called if the player loses the fight */
    protected virtual void LoseSequence()
    {

    }
}
