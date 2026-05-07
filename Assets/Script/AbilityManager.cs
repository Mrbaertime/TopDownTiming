using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("Data")]
    public List<AbilityData> allAbilities;

    [Header("Reference")]
    public GameObject panel;
    public GameObject player;
    public AbilityButtonUI[] buttons;

    private List<AbilityData> choices = new List<AbilityData>();
    private List<AbilityData> ownedAbilities = new List<AbilityData>();

    // =========================
    // 🎯 SHOW ABILITY
    // =========================
    public void ShowAbilities()
    {
        choices.Clear();

        List<AbilityData> available = new List<AbilityData>();

        foreach (var ab in allAbilities)
        {
            // 🔥 1. ถ้ามี "ตัว upgrade" นี้แล้ว → ห้ามเอาตัว base มา
            bool alreadyUpgraded = false;

            foreach (var owned in ownedAbilities)
            {
                if (owned == ab.upgradeTo)
                {
                    alreadyUpgraded = true;
                    break;
                }
            }

            if (alreadyUpgraded)
                continue;

            // 🆕 2. ยังไม่มี → ใช้ได้
            if (!ownedAbilities.Contains(ab))
            {
                available.Add(ab);
            }
            // 🔁 3. มีแล้ว → ถ้ามี upgrade → เอา upgrade มา
            else if (ab.upgradeTo != null)
            {
                // ❗ กันไม่ให้ Triple โผล่ซ้ำ
                if (!ownedAbilities.Contains(ab.upgradeTo))
                {
                    available.Add(ab.upgradeTo);
                }
            }
        }

        // ❌ ไม่มีอะไรให้เลือก
        if (available.Count == 0)
        {
            panel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        panel.SetActive(true);

        int count = Mathf.Min(3, available.Count);

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, available.Count);
            choices.Add(available[rand]);
            available.RemoveAt(rand);
        }

        // 🎮 ตั้งค่าปุ่ม UI
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

    // =========================
    // 🎯 CHECK AVAILABLE
    // =========================
    public bool HasAvailableAbility()
    {
        foreach (var ab in allAbilities)
        {
            if (!ownedAbilities.Contains(ab) || ab.upgradeTo != null)
                return true;
        }
        return false;
    }

    // =========================
    // 🎯 CHOOSE ABILITY
    // =========================
    public void ChooseAbility(int index)
    {
        // 🔥 กัน index หลุด
        if (index < 0 || index >= choices.Count)
            return;

        AbilityData selected = choices[index];

        AbilityData baseAbility = null;


        // 🔍 หา ability ที่อัปเกรดมาเป็นตัวนี้
        foreach (var ab in ownedAbilities)
        {
            if (ab.upgradeTo == selected)
            {
                baseAbility = ab;
                break;
            }
        }

        if (baseAbility != null)
        {
            // 🔁 อัปเกรด (Double → Triple)

            // ❗ ไม่ต้อง Remove ของเก่า
            if (!ownedAbilities.Contains(selected))
            {
                ownedAbilities.Add(selected);
            }

            ActivateAbility(selected);
        }
        else
        {
            // 🆕 ได้ ability ใหม่

            // ❗ กันซ้ำ
            if (!ownedAbilities.Contains(selected))
            {
                ownedAbilities.Add(selected);
                ActivateAbility(selected);
            }
        }

        // 🔒 ปิด UI + เล่นต่อ
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    // =========================
    // 🎯 ACTIVATE
    // =========================
    void ActivateAbility(AbilityData ability)
    {
        GameObject obj = Instantiate(ability.abilityPrefab, player.transform);

        PlayerAbility ab = obj.GetComponent<PlayerAbility>();

        if (ab != null)
        {
            ab.Activate(player);
        }
    }

    // =========================
    // ⏭ SKIP
    // =========================
    public void SkipUpgrade()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }


    // =========================
    //Save System
    // =========================
    public List<AbilityData> GetOwnedAbilities()
    {
        return ownedAbilities;
    }

    public void LoadAbilities(List<string> abilityNames)
    {
        foreach (string abName in abilityNames)
        {
            AbilityData found = allAbilities.Find(a => a.abilityName == abName);

            if (found != null)
            {
                if (!ownedAbilities.Contains(found))
                {
                    ownedAbilities.Add(found);

                    GameObject obj = Instantiate(found.abilityPrefab, player.transform);

                    PlayerAbility ab = obj.GetComponent<PlayerAbility>();

                    if (ab != null)
                    {
                        ab.Activate(player);
                    }
                }
            }
        }
    }
}

