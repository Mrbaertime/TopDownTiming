using UnityEngine;

public class BigBullet : PlayerAbility
{
    public float scaleMultiplier = 1.5f;

    public override void Activate(GameObject player)
    {
        var shoot = player.GetComponent<PlayerAimShoot>();
        shoot.bulletScale *= scaleMultiplier;
    }
}