using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;
    public float currentExp = 0;
    public float expToNext = 10;

    public AbilityManager abilityManager;

    public void AddExp(float amount)
    {
        currentExp += amount;

        if (currentExp >= expToNext)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;

        currentExp -= expToNext; // 🔥 สำคัญ (ไม่ใช่ = 0)
        expToNext *= 1.5f;

        Time.timeScale = 0f;
        abilityManager.ShowAbilities();
    }
}