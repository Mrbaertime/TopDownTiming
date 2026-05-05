using UnityEngine;

public class MagnetAbility : PlayerAbility
{
    public float radius = 2f;
    public float pullSpeed = 10f;

    public override void Activate(GameObject player)
    {
        MagnetRuntime magnet = player.GetComponent<MagnetRuntime>();

        if (magnet == null)
        {
            magnet = player.AddComponent<MagnetRuntime>();
        }

        magnet.radius += radius;
        magnet.pullSpeed += pullSpeed;
    }
}