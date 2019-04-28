using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value; // sell value at shop
    public Sprite itemSprite;

    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP;
    public bool affectMP;
    public bool affectStrength;

    [Header("Combat Effects")]
    public int weaponStrength;
    public int armorStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Use selected item
    public void Use(int charIndex)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charIndex];
        if (isItem)
        {
            // Increase player's HP from health potion
            if (affectHP)
            {
                selectedChar.currentHP += amountToChange;
                if (selectedChar.currentHP > selectedChar.totalHP)
                {
                    selectedChar.currentHP = selectedChar.totalHP;
                }
            }
            // Increase player's MP from mana potion
            if (affectMP)
            {
                selectedChar.currentMP += amountToChange;
                if (selectedChar.currentMP > selectedChar.totalMP)
                {
                    selectedChar.currentMP = selectedChar.totalMP;
                }
            }
            // Permanently increase player's strength
            if (affectStrength)
            {
                selectedChar.strength += amountToChange;
            }
        }
        // Set character's attack power to strength + weapon's power
        if (isWeapon)
        {
            if (selectedChar.equippedWeap != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWeap);
            }
            selectedChar.equippedWeap = itemName;
            selectedChar.attack = selectedChar.strength + weaponStrength;
        }
        // Set character's defense to armor value
        if (isArmor)
        {
            if (selectedChar.equippedArmor != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmor);
            }
            selectedChar.equippedArmor = itemName;
            selectedChar.armor = armorStrength;
            selectedChar.defense = armorStrength + selectedChar.strength;
        }
        GameManager.instance.RemoveItem(itemName);
    }
}
