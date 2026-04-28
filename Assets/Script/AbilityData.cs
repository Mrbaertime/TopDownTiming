using UnityEngine;

[CreateAssetMenu(menuName = "Ability")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public string description;
    public Sprite icon;

    public GameObject abilityPrefab;

    public AbilityData upgradeTo;

}