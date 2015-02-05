using UnityEngine;
using System.Collections;

public class EndlessSpeedMultiply : MonoBehaviour {

    private const float ENDLESS_MODE_SPEED_MULTIPLIER = 1.2f;

    void OnLevelWasLoaded()
    {
        GameParameters.instance.speedMultiplier = ENDLESS_MODE_SPEED_MULTIPLIER;
    }
}
