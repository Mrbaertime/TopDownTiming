using UnityEngine;

public class DoubleShot : PlayerAbility
{
    public override void Activate(GameObject player)
    {
        var shoot = player.GetComponent<PlayerAimShoot>();
        shoot.isDoubleShot = true;
    }
}