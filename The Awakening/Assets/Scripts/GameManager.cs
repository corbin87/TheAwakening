using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Create singleton
    public static GameManager instance;

    // GM Variables
    public CharStats[] playerStats;

    public bool gameMenuOpen, dialogActive;

    public string[] itemsHeld;
    public int[] numItems;
    public Item[] referenceItems;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        // Freeze player during appropriate times
        if (gameMenuOpen || dialogActive)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Heavy Armor");
            RemoveItem("Health Potion");
        }

        // Press "O" to save game data
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveData();
        }

        // Press "P" to load game data
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }

    // Item inventory management
    public Item GetItemInformation(string itemDescription)
    {
        foreach (Item refItem in referenceItems)
        {
            if (refItem.itemName == itemDescription)
            {
                return refItem; // Item found
            }
        }

        // No item found
        return null;
    }

    // Inventory sorting & cleanup
    public void SortItems()
    {
        bool keepSorting = true;
        while (keepSorting)
        {
            keepSorting = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numItems[i] = numItems[i + 1];
                    numItems[i + 1] = 0;

                    if (itemsHeld[i] != "")
                    {
                        keepSorting = true;
                    }
                }
            }
        }
    }

    // Adding items to inventory
    public void AddItem(string itemToAdd)
    {
        // Does item exist to increment, or do we add new item to inventory screen?
        int newItemPosition = 0;
        bool foundSpot = false;
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                foundSpot = true;
                break;
            }
        }
        if (foundSpot)
        {
            // Add to inventory
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {
                // Check item name is valid
                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    break;
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError("Attempted to add invalid item " + itemToAdd + " to inventory.");
            }
        }

        GameMenu.instance.ShowItems();
    }

    // Remove items from inventory
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;
                break;
            }
        }

        if (foundItem)
        {
            numItems[itemPosition]--;
            if (numItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }
            GameMenu.instance.ShowItems();
        }
        else
        {
            // Error checking
            Debug.LogError("Attempted to remove invalid item " + itemToRemove + " from inventory.");
        }
    }

    // Saving and loading game data
    public void SaveData()
    {
        // Save current scene and player position in scene
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        // Save character information
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].playerExperience);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].totalHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].totalMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defense);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].attack);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armor);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWeap);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmor);
        }

        // Store inventory information
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }
    public void LoadData()
    {
        // Load player position
        PlayerController.instance.transform.position = new Vector3(
            PlayerPrefs.GetFloat("Player_Position_x"),
            PlayerPrefs.GetFloat("Player_Position_y"),
            PlayerPrefs.GetFloat("Player_Position_z"));

        // Load player stats
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].playerExperience = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].totalHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].totalMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defense = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].attack = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armor = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWeap = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equippedArmor = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
        }

        // Load player inventory
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }
}
