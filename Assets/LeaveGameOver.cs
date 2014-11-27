using UnityEngine;
using System.Collections;

public class LeaveGameOver : MonoBehaviour {

    public string sceneToLoad;
    GameParameters gParams;

    void Awake()
    {
        try
        {
            gParams = GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>();
        }
        catch (System.NullReferenceException)
        {
            gParams = null;
        }
        
    }

    void OnMouseDown()
    {
        if(gParams != null)
            Destroy(gParams.gameObject);
        Application.LoadLevel(sceneToLoad);
    }
}
