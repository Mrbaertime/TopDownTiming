using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int level;
    public float currentExp;
    public float expToNext;

    public int currentHP;

    public List<string> ownedAbilities = new List<string>();

    public float timer;
    public float bossTimer;
}