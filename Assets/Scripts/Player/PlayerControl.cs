using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    /* constants */
    private const float BASE_FIRE_RATE = 0.3f; // the base fire rate of the ship.  Lower is faster.
    private const float FIRE_RATE_PERK = 0.1f; // the rapid fire rate of the ship
    private const float SHOT_OFFSET = 1.5f; // the offset from the center of the transform (where the shot originates from)
    private const float RAPID_FIRE_TIMER = 10f; // the length of time for rapid fire to last
    private const float INVINCIBLE_TIME = 1f; // the time the player is invincible for after respawning
    private const float MAX_BANK_ANGLE = 20; // maximum bank of ship in degrees

    /* movement */
    public float strafeForce = 10f;
    private float strafeDirection;
    private float velocityDirection;
    public float velocityAddition = 2f;
    private bool canMove = true;
    public bool moveOverride = false;
    public Vector3 movementOverrideVector;

    /* weapons */
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
    private GameObject laser;
    private GameObject scatter;
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
    private ParticleSystem.EmissionModule engineParticleEmission;

    /* particles */
    private ParticleSystem explosionParticles;

    /* initialize parameters here */
    void Awake()
    {
        /* references to player objects */
        playerRenderer = GameObject.Find("PlayerShip_0").GetComponentsInChildren<MeshRenderer>();
        playerCollider = GameObject.Find("PlayerCollider").GetComponent<MeshCollider>();
        
        /* resources */
        playerExplosionSound = Resources.Load<AudioClip>("SoundFX/playerExplosion/explodey");
        laser = Resources.Load<GameObject>("Ammo/LaserShot");
        laserSound = Resources.Load<AudioClip>("SoundFX/laser/railgun");
        scatter = Resources.Load<GameObject>("Ammo/PlayerScatterLaser");
        bomb = Resources.Load<GameObject>("Ammo/Bomb");
        explosionParticles = Resources.Load<ParticleSystem>("Particles/Explosion");

        /* regular weapons */
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
        /* set game parameters */
        gParams = GameParameters.instance;
        pauseDisplay = GameObject.Find("PauseDisplay");
        pauseDisplay.GetComponent<Renderer>().enabled = false;
        gParams.lastCheckpoint = Vector3.zero;

        /* set up particle systems */
        engineParticleEmission = engineParticles.emission;

        StartCoroutine(PauseLoop()); // start the loop for pause function
    }
    
    void FixedUpdate()
    {

        if(canMove){
            strafeDirection = Input.GetAxis("Horizontal");
            velocityDirection = Input.GetAxis("Vertical");
        }

        /* strafing */
        GetComponent<Rigidbody>().AddForce(new Vector3(strafeDirection * strafeForce, 0, 0));
        /* forward and backward movement */
        GetComponent<Rigidbody>().velocity = new Vector3(0, velocityAddition * velocityDirection, 0);

        BankShip();
        
        /* movement override control */
        if (moveOverride)
        {
            transform.position = Vector3.MoveTowards(transform.position, movementOverrideVector, 0.07f);
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

    private void BankShip()
    {
        
        float currentAngle = MAX_BANK_ANGLE * Input.GetAxis("Horizontal");
        gameObject.transform.rotation = Quaternion.Euler(0, -currentAngle, 0);
    }

    /* handle collisions */
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Enemy" || col.collider.tag == "EnemyWeapon" || col.collider.tag == "Wall")
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
            GameObject clone = GameObject.Instantiate(laser, launchFrom, Quaternion.identity) as GameObject;
            clone.GetComponent<GenericAmmo>().shotVelocity += new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
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
                Vector3 launchFrom;

                if (specialWeapon == bomb)
                {
                    launchFrom = new Vector3(transform.position.x, transform.position.y + SHOT_OFFSET + bomb.GetComponent<SphereCollider>().radius, transform.position.z); // adds a little offset for the bomb
                }
                else
                {
                    launchFrom = new Vector3(transform.position.x, transform.position.y + SHOT_OFFSET, transform.position.z);
                }
                GameObject.Instantiate(type, launchFrom, Quaternion.identity);
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
    private void SetSpecialWeapon(PowerUpType weapon)
    {
        switch (weapon)
        {
            case PowerUpType.None:
                specialWeapon = null;
                specialWeaponAmmoCount = 0;
                break;
            case PowerUpType.Scatter:
                specialWeapon = scatter;
                specialWeaponAmmoCount = 6;
                break;
            case PowerUpType.Bomb:
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
                SetSpecialWeapon(type);
                shielded = false;
                SetRapidFire(false);
                break;
            case PowerUpType.Scatter:
                SetSpecialWeapon(type);
                specialFireRate = BASE_FIRE_RATE;
                shielded = false;
                break;
            case PowerUpType.Bomb:
                SetSpecialWeapon(type);
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

        if (type != PowerUpType.ExtraLife)
        {
            powerUpHUD.GetComponent<HUDPowerUp>().DisplayPowerUp(type);
        }
        
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
        canMove = false;
        strafeDirection = 0f;
        velocityDirection = 0f;
        canShoot = false;
        SetRenderers(false);
        playerCollider.enabled = false;
        engineParticleEmission.enabled = false;
    }

    /* enable player features when respawning */
    void EnablePlayer()
    {
        canMove = true;
        transform.position = gParams.lastCheckpoint;
        SetRenderers(true);
        playerCollider.enabled = true;
        engineParticleEmission.enabled = true;
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
        bool cheatCode = false;
        while (true)
        {
            if (Input.anyKeyDown && gParams.paused && cheatCode && !Input.GetKeyDown(KeyCode.L))
            {
                cheatCode = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gParams.paused)
                {
                    gParams.PauseGame();
                    pauseDisplay.GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    gParams.ResumeGame();
                    pauseDisplay.GetComponent<Renderer>().enabled = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Y) && gParams.paused)
            {
                gParams.ResumeGame();
                pauseDisplay.GetComponent<Renderer>().enabled = false;
                SceneManager.LoadScene("Menu_Game_Over");
            }
            if (Input.GetKeyDown(KeyCode.N) && gParams.paused)
            {
                gParams.ResumeGame();
                pauseDisplay.GetComponent<Renderer>().enabled = false;
            }
            if (Input.GetKeyDown(KeyCode.E) && gParams.paused)
            {
                cheatCode = true;
            }
            if (Input.GetKeyDown(KeyCode.L) && gParams.paused && cheatCode)
            {
                gParams.playerLives += 5;
                gParams.SetLivesText();
                cheatCode = false;
            }
            yield return null;
        }
    }

}
