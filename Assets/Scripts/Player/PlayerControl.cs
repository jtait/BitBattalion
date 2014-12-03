using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    /* constants */
    private const float BASE_FIRE_RATE = 0.3f; // the base fire rate of the ship.  Lower is faster.
    private const float FIRE_RATE_PERK = 0.1f; // the rapid fire rate of the ship
    private const float MISSILE_FIRE_RATE = 1f; // the missile fire rate
    private const float SHOT_OFFSET = 2.5f;
    private const float RAPID_FIRE_TIMER = 10f; // the length of time for rapid fire to last
    private const float INVINCIBLE_TIME = 1f; // the time the player is invincible for after respawning

    /* movement */
    public float strafeForce = 10f;
    private float strafeDirection;
    private float velocityDirection;
    public float velocityAddition = 2f;
    public bool moveOverride = false;
    public Vector3 movementOverrideVector;

    /* weapons */
    private GameObject ammo; // the ammo the player ship will fire
    private GameObject specialWeapon; // the special weapon (bomb, missile, etc.) that the player ship has
    private float fireRate; // delay between regular shots
    private float nextShot; // delay until next regualer shot can be fired
    private float specialFireRate; // delay between special shots
    private float nextSpecial; // delay until next special shot can be fired
    private bool canShoot = true;

    /* persistent game parameters */
    private GameParameters gParams;
    private GameObject pauseDisplay;
    private bool invincible = false; // is the player invincible

    /* types of available ammo */
    private enum WeaponType { laser, missile, bomb, none };
    private GameObject laser;
    private GameObject missile;
    private GameObject bomb;

    /* special weapons */
    private bool shielded = false;
    private int specialWeaponAmmoCount;
    private bool rapidFireEnabled;
    private float rapidFireTimer;
    private GameObject powerUpHUD;

    /* renderer and collider*/
    private MeshRenderer[] playerRenderer;
    private MeshCollider playerCollider;

    /* sound */
    private AudioClip playerExplosionSound;
    private AudioClip laserSound;

    /* shield */
    private ParticleSystem shieldParticles;
    private ParticleSystem engineParticles;

    /* particles */
    private GameObject explosionParticles;

    /* initialize parameters here */
    void Awake()
    {
        /* game parameters */
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        pauseDisplay = GameObject.Find("PauseDisplay");
        pauseDisplay.renderer.enabled = false;
        gParams.lastCheckpoint = Vector3.zero;

        /* references to player objects */
        playerRenderer = GameObject.Find("PlayerShip_0").GetComponentsInChildren<MeshRenderer>();
        playerCollider = GameObject.Find("PlayerCollider").GetComponent<MeshCollider>();
        
        /* resources */
        playerExplosionSound = Resources.Load<AudioClip>("SoundFX/playerExplosion/explodey");
        laser = Resources.Load<GameObject>("Ammo/LaserShot");
        laserSound = Resources.Load<AudioClip>("SoundFX/laser/railgun");
        missile = Resources.Load<GameObject>("Ammo/Missile");
        bomb = Resources.Load<GameObject>("Ammo/Bomb");
        explosionParticles = Resources.Load<GameObject>("Particles/Explosion");

        /* regular weapons */
        ammo = laser;
        nextShot = 0f;
        fireRate = BASE_FIRE_RATE;

        /* special weapons */
        specialWeaponAmmoCount = 0;
        rapidFireEnabled = false;
        rapidFireTimer = 0f;
        nextSpecial = 0f;
        powerUpHUD = GameObject.Find("PowerUpDisplay");

        /* particles */
        shieldParticles = GameObject.Find("ShieldParticles").GetComponent<ParticleSystem>();
        engineParticles = GameObject.Find("ShipEngineParticles").GetComponent<ParticleSystem>();

    }

    void Start()
    {
        StartCoroutine(PauseLoop()); // start the loop for pause function
    }
    
    void FixedUpdate()
    {
        /* strafing */
        strafeDirection = Input.GetAxis("Horizontal");
        rigidbody.AddForce(new Vector3(strafeDirection * strafeForce, 0, 0));

        /* forward and backward movement */
        velocityDirection = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector3(0, velocityAddition * velocityDirection, 0);

        /* movement override control */
        if (moveOverride)
        {
            transform.position = Vector3.MoveTowards(transform.position, movementOverrideVector, 0.1f);
        }

        /* shield display */
        if (shielded)
        {
            shieldParticles.Play();
        }
        else
        {
            shieldParticles.Stop();
        }
    }

    /* handle collisions */
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Enemy" || col.collider.tag == "EnemyWeapon")
        {
            if (col.collider.tag == "EnemyWeapon")
            {
                Destroy(col.gameObject);
            }

            if (!invincible)
            {
                if (shielded)
                {
                    shielded = false;
                    PowerUpPickup(PowerUpType.None);
                }
                else PlayerDeath();
            }
        }
    }

    /* controls and timers */
    void Update()
    {
        if (Input.GetAxis("Fire1") == 1)
        {
            Shoot();
        }
        if (Input.GetAxis("Fire2") == 1)
        {
            ShootSpecialWeapon(specialWeapon);
        }

        /* rapid fire timer */
        if (rapidFireEnabled)
        {
            rapidFireTimer -= Time.deltaTime;
            if (rapidFireTimer < 0)
            {
                SetRapidFire(false);
                PowerUpPickup(PowerUpType.None);
            }
        }
    }

    /* create and launch projectile from ship's location */
    void Shoot()
    {
        if (Time.time > nextShot && canShoot)
        {
            // generate a new object to fire, instantiate with velocity, power, etc.
            Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + SHOT_OFFSET, transform.position.z);
            GameObject clone = GameObject.Instantiate(ammo, launchFrom, Quaternion.identity) as GameObject;
            clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
            nextShot = Time.time + fireRate;
            AudioSource.PlayClipAtPoint(laserSound, transform.position, 0.4f);
        }
    }

    /* create and fire a special weapon if the player has one */
    void ShootSpecialWeapon(GameObject type)
    {
        if (specialWeapon != null && canShoot)
        {
            if (Time.time > nextSpecial)
            {
                Vector3 launchFrom = new Vector3(transform.position.x, transform.position.y + SHOT_OFFSET, transform.position.z);
                GameObject clone = GameObject.Instantiate(type, launchFrom, Quaternion.identity) as GameObject;
                clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, rigidbody.velocity.y, 0);
                nextSpecial = Time.time + specialFireRate;
                specialWeaponAmmoCount--;
                if (specialWeaponAmmoCount <= 0)
                    PowerUpPickup(PowerUpType.None);
            }
        }
    }

    /* called when a player dies */
    public void PlayerDeath()
    {
        AudioSource.PlayClipAtPoint(playerExplosionSound, transform.position, 1f); // play explosion sound
        GameObject.Instantiate(explosionParticles, transform.position, Quaternion.identity); // play explosion particles

        invincible = true;

        /* decrease player lives */
        gParams.playerLives--;
        gParams.SetLivesText();

        /* reset powerUp benefits */
        if (!gParams.endlessMode)
            PowerUpPickup(PowerUpType.None);

        /* endless mode special */
        if (gParams.endlessMode)
        {
            gParams.DestroyAllEnemiesInList();
        }

        if (gParams.playerLives > 0)
        {
            /* respawn player at last checkpoint */
            StartCoroutine(WaitForRespawn(2));
            DisablePlayer();
        }
        else
        {
            /* GAME OVER */
            DisablePlayer();
            StartCoroutine(GameParameters.WaitForLevelLoad(2, "Menu_Game_Over")); // wait for 2 seconds and load the game over screen
        }
    }

    void OnTriggerEnter(Collider col)
    {
        /* update checkpoints as the player reaches them */
        if (col.tag == "CheckPoint")
        {
            gParams.lastCheckpoint = col.transform.position;
        }
    }

    /* set the special weapon parameters to the specified WeaponType */
    private void SetSpecialWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.none:
                specialWeapon = null;
                specialWeaponAmmoCount = 0;
                break;
            case WeaponType.missile:
                specialWeapon = missile;
                specialWeaponAmmoCount = 6;
                break;
            case WeaponType.bomb:
                specialWeapon = bomb;
                specialWeaponAmmoCount = 1;
                break;
        }
    }

    /* set rapid fire parameters */
    private void SetRapidFire(bool set)
    {
        rapidFireEnabled = set;
        if (set)
        {
            fireRate = FIRE_RATE_PERK;
            rapidFireTimer = RAPID_FIRE_TIMER;
        }
        else
        {
            fireRate = BASE_FIRE_RATE;
            rapidFireTimer = 0;
        }
    }

    /* handle state changes when picking up a powerup */
    public void PowerUpPickup(PowerUpType type)
    {
        switch(type){
            case PowerUpType.None:
                ammo = laser;
                SetSpecialWeapon(WeaponType.none);
                shielded = false;
                SetRapidFire(false);
                break;
            case PowerUpType.Missile:
                SetSpecialWeapon(WeaponType.missile);
                specialFireRate = MISSILE_FIRE_RATE;
                shielded = false;
                break;
            case PowerUpType.Bomb:
                SetSpecialWeapon(WeaponType.bomb);
                shielded = false;
                break;
            case PowerUpType.ExtraLife:
                gParams.playerLives++;
                gParams.SetLivesText();
                break;
            case PowerUpType.RapidFire:
                SetRapidFire(true);
                shielded = false;
                break;
            case PowerUpType.Shield:
                shielded = true;
                break;
        }
        
        powerUpHUD.GetComponent<HUDPowerUp>().DisplayPowerUp(type);
        
    }

    IEnumerator WaitForRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.parent.position = gParams.lastCheckpoint;
        EnablePlayer();
        StartCoroutine(InvincibleTimer(INVINCIBLE_TIME));
    }

    /* disable player features on death */
    void DisablePlayer()
    {
        canShoot = false;
        SetRenderers(false);
        playerCollider.enabled = false;
        engineParticles.enableEmission = false;
    }

    /* enable player features when respawning */
    void EnablePlayer()
    {
        transform.position = gParams.lastCheckpoint;
        SetRenderers(true);
        playerCollider.enabled = true;
        engineParticles.enableEmission = true;
        canShoot = true;
    }

    /* set player invincible to false after time */
    IEnumerator InvincibleTimer(float time)
    {
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    private void SetRenderers(bool set)
    {
        foreach (Renderer r in playerRenderer)
        {
            r.enabled = set;
        }
    }

    /* loop to enable pausing */
    IEnumerator PauseLoop()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gParams.paused)
                {
                    gParams.PauseGame();
                    pauseDisplay.renderer.enabled = true;
                }
                else
                {
                    gParams.ResumeGame();
                    pauseDisplay.renderer.enabled = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Y) && gParams.paused)
            {
                gParams.ResumeGame();
                pauseDisplay.renderer.enabled = false;
                Application.LoadLevel("Menu_Game_Over");
            }
            if (Input.GetKeyDown(KeyCode.N) && gParams.paused)
            {
                gParams.ResumeGame();
                pauseDisplay.renderer.enabled = false;
            }
            yield return null;
        }
    }

}
