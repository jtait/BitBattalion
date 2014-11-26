using UnityEngine;
using System.Collections;

public class Firewall_0 : GenericEnemy {

    private const float BASE_WALL_ACTIVE_TIME = 0.5f; // the number of seconds the wall is off
    private const float BASE_WALL_INACTIVE_TIME = 2f; // the number of seconds the wall is on

    private BoxCollider wallCollider; // the collider for the wall
    private ParticleSystem[] fireParticles; // the particle systems for the fire
    private float wallActiveTime; // the duration in seconds that the wall is active
    private float wallInactiveTime; // the duration in seconds the the wall is inactive
    public bool cycle = true; // is the wall currently cycling?
    
	protected override void Start () {
        wallInactiveTime = BASE_WALL_INACTIVE_TIME / difficulty;
        wallActiveTime = BASE_WALL_ACTIVE_TIME * difficulty;
        wallCollider = GetComponent<BoxCollider>();
        fireParticles = GetComponentsInChildren<ParticleSystem>();

        /* start the wall */
        StartCoroutine(Wall());
	}
	
    /* turns wall on and off */
    IEnumerator Wall()
    {
        while (cycle)
        {
            SetParticles(true);
            wallCollider.enabled = true;
            yield return new WaitForSeconds(wallActiveTime);
            SetParticles(false);
            wallCollider.enabled = false;
            yield return new WaitForSeconds(wallInactiveTime);
        }
    }

    /* helper to set both ParticleSystems at once */
    private void SetParticles(bool active)
    {
        foreach(ParticleSystem p in fireParticles){
            p.enableEmission = active;
        }
    }

    /* override base to make indestructible */
    protected override void OnCollisionEnter(Collision col)
    {
    }

}
