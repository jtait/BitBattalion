using UnityEngine;
using System.Collections;

public class BossTube : GenericBoss {

    private const int BASE_HEALTH = 150; // the base health of the tube
    private const int ALLOWABLE_ESCAPED_BITS = 14; // the number of bits that can escape before the player loses
    private int escapedBits = 0; // keeps track of the number of escaped bits
    private const int BASE_POINTS = 4000;

    private TextMesh bitCounter;
    private Spawner bitSpawner1, bitSpawner2;
    private bool previousFrame = false; // the active state of the tube in the previous frame

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
        activateDistance = 14.6f;
        nextLevel = "Story_Level_02";
	}

    protected override void Update()
    {
        base.Update();

        if (previousFrame != bossActive)
        {
            escapedBits = 0; // reset bits when boss goes active
        }
        previousFrame = bossActive;
        UpdateBitCounter(); // update the text

        /* check if spawning */
        SpawnBits(bossActive);

        /* check if the player has lost */
        if (escapedBits >= ALLOWABLE_ESCAPED_BITS)
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
         bitCounter.text = "Escaped Bits Before Total Infection: " + (ALLOWABLE_ESCAPED_BITS - escapedBits);
         if (ALLOWABLE_ESCAPED_BITS - escapedBits > 4) bitCounter.color = Color.white;
         else bitCounter.color = Color.red;
    }

    /* enables and disables spawners in boss area */
    private void SpawnBits(bool set)
    {
        bitSpawner1.spawn = set;
        bitSpawner2.spawn = set;
    }

    /* called if the player loses the fight */
    public override void LoseSequence()
    {
        base.LoseSequence();
        escapedBits = 0; // reset escaped bits
    }

}
