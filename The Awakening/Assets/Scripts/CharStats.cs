using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    // Player status
    public string charName;
    public Sprite charImage;
    public int playerLevel = 1;
    public int playerExperience;
    public int baseXP = 0;
    public int[] experienceToLevel;
    public int maxLevel = 50;
    public int totalHP = 100;
    public int totalMP = 100;
    public int currentHP = 100;
    public int currentMP = 100;

    // Player attributes
    public int strength;
    public int defense;
    public int attack;
    public int armor;

    // Player items
    public string equippedWeap;
    public string equippedArmor;

    // Start is called before the first frame update
    void Start()
    {
        // Set required experience
        experienceToLevel = new int[maxLevel];
        experienceToLevel[0] = baseXP;
        for (int i = 1; i < experienceToLevel.Length; i++)
        {
            experienceToLevel[i] = experienceToLevel[i-1] + 100;
        }
        attack = strength;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            addExperience(1000);
        }
    }

    public void addExperience(int earnedXP)
    {
        playerExperience += earnedXP;

        // Level character when sufficient XP earned
        if (playerLevel < maxLevel)
        {
            if (playerExperience >= experienceToLevel[playerLevel])
            {
                playerExperience = 0;
                playerLevel++;

                // Increase player stats upon leveling
                strength += 5;
                defense += 5;
                totalHP += 100;
                totalMP += 100;
                currentHP = totalHP;
                currentMP = totalMP;
            }
        }
        else
        {
            playerExperience = 0;
        }
    }
}
