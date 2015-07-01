using UnityEngine;
using System.Collections;

public class TopOfScreenTrigger : MonoBehaviour {

    GameParameters gParams;

    void Start()
    {
        gParams = GameParameters.instance;
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
