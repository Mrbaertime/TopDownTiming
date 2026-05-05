using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_HP : MonoBehaviour
{
    [Header("UI Hearts")]
    public List<Image> hearts; // จำนวนหัวใจใน UI (เช่น 5 ดวง)

    [Header("Player")]
    public Health playerHealth;

    private int lastHeartCount = -1;

    void Update()
    {
        int currentHP = GetCurrentHP();
        int maxHP = playerHealth.maxHealth;

        // 🔥 แปลง HP → จำนวนหัวใจ
        int heartCount = Mathf.CeilToInt((float)currentHP / maxHP * hearts.Count);

        if (heartCount != lastHeartCount)
        {
            UpdateHearts(heartCount);
            lastHeartCount = heartCount;
        }
    }

    void UpdateHearts(int activeHearts)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < activeHearts);
        }
    }

    // 👉 อ่าน currentHealth โดยไม่แก้ Health.cs
    int GetCurrentHP()
    {
        var field = typeof(Health).GetField("currentHealth",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        return (int)field.GetValue(playerHealth);
    }
}