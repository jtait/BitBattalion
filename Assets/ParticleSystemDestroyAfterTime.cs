using UnityEngine;
using System.Collections;

public class ParticleSystemDestroyAfterTime : MonoBehaviour {

	void Start () {
        StartCoroutine(DestroyAfter(particleSystem.duration + particleSystem.startLifetime));
	}
	
    IEnumerator DestroyAfter(float time){
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
	
}
