using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButtonUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descText;

    public int index;
    public AbilityManager manager;

    public void Setup(AbilityData data, int i)
    {
        index = i;

        icon.sprite = data.icon;
        nameText.text = data.abilityName;
        descText.text = data.description;
    }

    public void OnClick()
    {
        manager.ChooseAbility(index);
    }
}