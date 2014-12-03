using UnityEngine;
using System.Collections;

public class EndlessSpeedMultiply : MonoBehaviour {

    private const float ENDLESS_MODE_SPEED_MULTIPLIER = 1.2f;

    void OnLevelWasLoaded()
    {
        GameObject.FindGameObjectWithTag("GameParameters").GetComponent<GameParameters>().speedMultiplier = ENDLESS_MODE_SPEED_MULTIPLIER;
    }
}
