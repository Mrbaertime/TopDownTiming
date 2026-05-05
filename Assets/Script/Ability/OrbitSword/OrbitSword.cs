using UnityEngine;

public class OrbitSword : PlayerAbility
{
    public GameObject swordPrefab;
    public int swordCount = 2;

    public override void Activate(GameObject player)
    {
        for (int i = 0; i < swordCount; i++)
        {
            GameObject sword = Instantiate(swordPrefab, player.transform);

            OrbitSwordRuntime orbit = sword.GetComponent<OrbitSwordRuntime>();

            orbit.center = player.transform; // 🔥 ตัวนี้แหละสำคัญ
            orbit.index = i;
            orbit.total = swordCount;
        }
    }
}