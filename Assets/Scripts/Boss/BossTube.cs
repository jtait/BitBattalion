using UnityEngine;
using System.Collections;

public class BossTube : GenericBoss {

    private const int BASE_HEALTH = 150; // the base health of the tube
    private int allowableEscapedBits; // the number of bits that can escape before the player loses
    private int escapedBits = 0; // keeps track of the number of escaped bits
    private const int BASE_POINTS = 4000;

    private TextMesh bitCounter;
    private Spawner bitSpawner1, bitSpawner2;

    protected override void Awake()
    {
        base.Awake();
        bitSpawner1 = GameObject.Find("BossBitSpawner_01").GetComponent<Spawner>();
        bitSpawner2 = GameObject.Find("BossBitSpawner_02").GetComponent<Spawner>();
        bitCounter = GameObject.Find("BitCounter").GetComponent<TextMesh>();
    }

	protected override void Start () {
        points = BASE_POINTS * difficulty;
        health = BASE_HEALTH * gParams.difficulty / 2; // multiply boss health by difficulty level
        startHealth = health;
        allowableEscapedBits = 19 * (1 / gParams.difficulty); // arbitrary number
        activateDistance = 14.6f;
        nextLevel = "Story_Level_02";
	}

    protected override void Update()
    {
        base.Update();

        UpdateBitCounter(); // update the text

        /* check if spawning */
        if (bossActive)
        {
            SpawnBits(true);
        }
        else SpawnBits(false);

        /* check if the player has lost */
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

    /* update the bit counter text */
    private void UpdateBitCounter(){
         bitCounter.text = "Escaped Bits Before Total Infection: " + (allowableEscapedBits - escapedBits);
         if (allowableEscapedBits - escapedBits > 4) bitCounter.color = Color.white;
         else bitCounter.color = Color.red;
    }

    /* enables and disables spawners in boss area */
    private void SpawnBits(bool set)
    {
        bitSpawner1.spawn = set;
        bitSpawner2.spawn = set;
    }

}
