using UnityEngine;
using System.Collections;

public class BottomOfScreenTrigger : MonoBehaviour {

    GameParameters gParams;

    void Awake()
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
            gParams.RemoveEnemyFromList(col.gameObject);
        }
    }
}
