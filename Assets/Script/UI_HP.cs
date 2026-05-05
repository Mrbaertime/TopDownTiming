using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

public class UI_HP : MonoBehaviour
{
    [Header("UI Hearts")]
    public List<Image> hearts; // ลาก Image หัวใจมาใส่ (เช่น 5 อัน)

    [Header("Player")]
    public Health playerHealth;

    [Header("Visual")]
    public Color fullColor = Color.white;                  // ตอนเต็ม
    public Color emptyColor = new Color(1, 1, 1, 0.2f);    // ตอนใกล้หมด (โปร่ง)

    private int lastHP = -1;

    void Update()
    {
        int currentHP = GetCurrentHP();
        int maxHP = playerHealth.maxHealth;

        if (currentHP != lastHP)
        {
            UpdateHearts(currentHP, maxHP);
            lastHP = currentHP;
        }
    }

    // =========================
    // ❤️ อัปเดตความจางของหัวใจ
    // =========================
    void UpdateHearts(int currentHP, int maxHP)
    {
        float hpPercent = (float)currentHP / maxHP;

        for (int i = 0; i < hearts.Count; i++)
        {
            float threshold = (float)(i + 1) / hearts.Count;

            // ถ้า HP ยังถึงช่วงของหัวใจดวงนี้ → เต็ม
            if (hpPercent >= threshold)
            {
                hearts[i].color = fullColor;
            }
            else
            {
                // 🔥 คำนวณความจางแบบ smooth
                float prevThreshold = (float)i / hearts.Count;

                float t = Mathf.InverseLerp(prevThreshold, threshold, hpPercent);

                hearts[i].color = Color.Lerp(emptyColor, fullColor, t);
            }
        }
    }

    // =========================
    // 🧠 อ่าน currentHealth (ไม่แก้ Health.cs)
    // =========================
    int GetCurrentHP()
    {
        var field = typeof(Health).GetField("currentHealth",
            BindingFlags.NonPublic | BindingFlags.Instance);

        return (int)field.GetValue(playerHealth);
    }
}