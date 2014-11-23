using UnityEngine;
using System.Collections;

public class TopOfScreenTrigger : MonoBehaviour {

    GameParameters gParams;

    void Awake()
    {
        gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Weapon")
        {
            Destroy(col.gameObject);
        }

        if (col.tag == "Enemy")
        {
            gParams.AddEnemyToList(col.gameObject);
        }
    }
}
