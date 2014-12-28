using UnityEngine;
using System.Collections;

public abstract class GenericBoss : MonoBehaviour {

    protected int health;
    protected int startHealth;
    protected GameParameters gParams;
    protected const float WAIT_TIME_FOR_LEVEL_LOAD = 3;
    protected Transform playerTransform; // the transform of the player
    protected int difficulty;
    protected bool bossActive = false; // is the boss active?
    protected float activateDistance; // the proximity of the player before becoming active
    protected int points; // the number of points awarded for killing the boss
    protected string nextLevel; // the level to load after defeating the boss
    protected bool canBeDestroyed = true;
    protected bool dead = false;

    protected GameObject healthBar;

    /* renderer */
    private MeshRenderer[] bossRenderer;

    protected virtual void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        bossRenderer = GetComponentsInChildren<MeshRenderer>();
        healthBar = GameObject.Find("BossHealthBar");
        difficulty = gParams.difficulty;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {

    }
    
	protected virtual void Update ()
    {
        DeathCheck();
        if (playerTransform.position.y >= transform.position.y - activateDistance - 0.1f)
        {
            bossActive = true;
            DisplayHealthBar(true);
        }
        else if (playerTransform.position.y < transform.position.y - activateDistance)
        {
            bossActive = false;
            DisplayHealthBar(false);
        }

        UpdateHealthBar();
	}

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (canBeDestroyed)
        {
            if (col.collider.tag == "Weapon")
            {
                health--;
            }
        }
    }

    protected virtual void DeathCheck()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            DeathSequence();
        }
    }

    /* called if the player wins and the boss dies */
    protected virtual void DeathSequence()
    {
        SetRenderers(false);
        collider.enabled = false;
        gParams.UpdateScore(points);
        DestroyAllEnemiesAndSpawners();
        StartCoroutine(GameParameters.WaitForLevelLoad(WAIT_TIME_FOR_LEVEL_LOAD, nextLevel));       
    }

    /* called if the player loses the fight */
    public virtual void LoseSequence()
    {
        StartCoroutine(InvincibleForSeconds(2));
        playerTransform.GetComponent<PlayerControl>().PlayerDeath();
        health = startHealth; // reset boss health
    }

    protected void DestroyAllEnemiesAndSpawners()
    {
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("Enemy"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("Spawner"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("EnemyWeapon"));
        DestroyGameObjectsInArray(GameObject.FindGameObjectsWithTag("BossMinion"));   
    }

    private void DestroyGameObjectsInArray(GameObject[] array)
    {
        foreach (GameObject enemy in array)
        {
            if (enemy != null)
                Destroy(enemy);
        }
    }

    private void SetRenderers(bool set)
    {
        foreach (Renderer r in bossRenderer)
        {
            r.enabled = set;
        }
    }

    IEnumerator InvincibleForSeconds(float seconds)
    {
        canBeDestroyed = false;
        yield return new WaitForSeconds(seconds);
        canBeDestroyed = true;
    }

    protected void DisplayHealthBar(bool set){
        healthBar.renderer.enabled = set;
    }


    protected void UpdateHealthBar(){
        healthBar.transform.localScale = new Vector3((((float)health) / ((float)startHealth)), 1f, 1f);
    }

}
