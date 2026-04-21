using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<AbilityData> allAbilities;
    public GameObject panel;
    public GameObject player;

    private List<AbilityData> choices = new List<AbilityData>();
    private List<AbilityData> ownedAbilities = new List<AbilityData>();

    public AbilityButtonUI[] buttons;

    public void ShowAbilities()
    {
        panel.SetActive(true);
        choices.Clear();

        List<AbilityData> available = new List<AbilityData>();

        foreach (var ab in allAbilities)
        {
            if (!ownedAbilities.Contains(ab))
                available.Add(ab);
        }

        // 👉 สุ่มแบบ "ไม่เกินจำนวนที่มี"
        int count = Mathf.Min(3, available.Count);

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, available.Count);
            choices.Add(available[rand]);
            available.RemoveAt(rand);
        }

        // 🔥 สำคัญ: เปิด/ปิดปุ่มให้ตรงจำนวน
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < choices.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].Setup(choices[i], i);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChooseAbility(int index)
    {
        AbilityData selected = choices[index];

        ownedAbilities.Add(selected);

        GameObject obj = Instantiate(selected.abilityPrefab, player.transform);
        obj.GetComponent<PlayerAbility>().Activate(player);

        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SkipUpgrade()
    {
        panel.SetActive(false);
        Time.timeScale = 1f; // ▶️ เล่นต่อ
    }
}