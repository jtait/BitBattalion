using UnityEngine;
using System.Collections;

public class HUDPowerUp : MonoBehaviour {

    private Renderer tile;

    private Material scatter;
    private Material bomb;
    private Material rapidfire;
    private Material shield;
    private Material none;

    void Awake()
    {
        tile = GetComponent<MeshRenderer>();
        none = Resources.Load<Material>("Powerups/pu_none");
        scatter = Resources.Load<Material>("Powerups/pu_scatter");
        bomb = Resources.Load<Material>("Powerups/pu_bomb");
        rapidfire = Resources.Load<Material>("Powerups/pu_rapidfire");
        shield = Resources.Load<Material>("Powerups/pu_shield");
    }

	public void DisplayPowerUp(PowerUpType type){
        switch (type)
        {
            case PowerUpType.Scatter:
                tile.material = scatter;
                break;
            case PowerUpType.Bomb:
                tile.material = bomb;
                break;
            case PowerUpType.RapidFire:
                tile.material = rapidfire;
                break;
            case PowerUpType.Shield:
                tile.material = shield;
                break;
            default:
                tile.material = none;
                break;
        }
    }
}
