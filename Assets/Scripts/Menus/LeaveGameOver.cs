using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LeaveGameOver : MonoBehaviour {

    public string sceneToLoad;
    GameParameters gParams;

    void Awake()
    {
        try
        {
            gParams = GameParameters.instance;
            gParams.speedMultiplier = 1; // reset speed multiplier
        }
        catch (System.NullReferenceException)
        {
            gParams = null;
        }
        
    }

    void OnMouseDown()
    {
        Leave();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Leave();
        }
    }

    void Leave()
    {
        if (gParams != null)
            Destroy(gParams.gameObject);
        SceneManager.LoadScene(sceneToLoad);
    }

}
