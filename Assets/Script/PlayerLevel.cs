using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level")]
    public int level = 1;
    public float currentExp = 0;
    public float expToNext = 10;

    [Header("Reference")]
    public AbilityManager abilityManager;

    public void AddExp(float amount)
    {
        currentExp += amount;

        // 🔥 รองรับ EXP เกินหลายเลเวล
        while (currentExp >= expToNext)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;

        // 🔥 เก็บ exp ส่วนเกินไว้
        currentExp -= expToNext;

        // 🔥 เพิ่มความยาก
        expToNext *= 1.5f;

        // 🔥 เช็คก่อนเปิด UI
        if (abilityManager != null && abilityManager.HasAvailableAbility())
        {
            Time.timeScale = 0f;
            abilityManager.ShowAbilities();
        }
        else
        {
            // ❗ ไม่มี ability → เล่นต่อ
            Time.timeScale = 1f;
        }
    }
}