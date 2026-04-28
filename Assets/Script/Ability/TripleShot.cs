using UnityEngine;

public class TripleShot : PlayerAbility
{
    public override void Activate(GameObject player)
    {
        var shoot = player.GetComponent<PlayerAimShoot>();

        shoot.isDoubleShot = false; // 🔥 ปิดของเก่า
        shoot.isTripleShot = true;  // 🔥 เปิดใหม่
    }
}
