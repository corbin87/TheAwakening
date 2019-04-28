using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // singleton and convenience references
    public static GameMenu instance;
    public GameObject menu;
    public GameObject[] windows;
    private CharStats[] playerStats;
    public string mainMenuName;

    // Character stats variables
    public Text[] nameText, hpText, mpText, levelText, xpText;
    public Slider[] xpSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;

    // Inventory variables
    public ItemButton[] itemButtons;
    public string[] selectedItems;
    public Item activeItem;
    public Text itemName;
    public Text itemDescription;
    public Text useButtonText;
    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    // Status window variables
    public GameObject[] statusButtons;
    public Text statusName, statusHP, statusMP, statusStrength, statusDefense, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;
    public Image statusImage;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (menu.activeInHierarchy)
            {
                CloseMenu();
            }
            else
            {
                UpdateMainStats();
                menu.SetActive(true);
                GameManager.instance.gameMenuOpen = true;
            }
        }
    }

    // Link private ivar to char stats array
    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);
                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].totalHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].totalMP;
                levelText[i].text = "Level: " + playerStats[i].playerLevel;
                xpText[i].text = "" + playerStats[i].playerExperience + "/" + playerStats[i].experienceToLevel[playerStats[i].playerLevel];
                xpSlider[i].maxValue = playerStats[i].experienceToLevel[playerStats[i].playerLevel];
                xpSlider[i].value = playerStats[i].playerExperience;
                charImage[i].sprite = playerStats[i].charImage;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }
    }

    // Show other game menu options
    public void ToggleWindow(int windowNum)
    {
        UpdateMainStats();
        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowNum)
            {
                if (i == 0)
                {
                    // Items window - sort first
                    ShowItems();
                }
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
        itemCharChoiceMenu.SetActive(false);
    }

    // Close menu on button click
    public void CloseMenu()
    {
        // Close all submenus
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        // Close main game menu
        menu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
        itemCharChoiceMenu.SetActive(false);
    }

    // Sort and show inventory items
    public void ShowItems()
    {
        GameManager.instance.SortItems();
        int i = 0;
        foreach (ItemButton btn in itemButtons)
        {
            btn.buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemInformation(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
            i++;
        }
    }

    public void SelectItem(Item item)
    {
        activeItem = item;

        // Change button text display based on item selected
        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }
        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    // Remove item from the inventory
    public void DiscardItem()
    {
        GameManager.instance.RemoveItem(activeItem.itemName);
    }

    // Use item for which player?
    public void OpenItemCharSelect()
    {
        itemCharChoiceMenu.SetActive(true);
        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }
    public void CloseItemCharSelect()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    // Uses item and updates player stats accordingly
    public void UseItem(int selectedChar)
    {
        activeItem.Use(selectedChar);
        CloseItemCharSelect();
        UpdateMainStats();
    }

    // Showing the status menu
    public void OpenStatus()
    {
        UpdateMainStats();
        StatusChar(0);
        for (int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    // Update the status screen with character stat sheet
    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].totalHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].totalMP;
        statusStrength.text = playerStats[selected].strength.ToString();
        statusDefense.text = playerStats[selected].defense.ToString();
        if (playerStats[selected].equippedWeap != "")
        {
            statusWpnEqpd.text = playerStats[selected].equippedWeap;
        } else
        {
            statusWpnEqpd.text = "None";
        }
        statusWpnPwr.text = playerStats[selected].attack.ToString();
        if (playerStats[selected].equippedArmor != "")
        {
            statusArmrEqpd.text = playerStats[selected].equippedArmor;
        }
        else
        {
            statusArmrEqpd.text = "None";
        }
        statusArmrPwr.text = playerStats[selected].armor.ToString();
        statusExp.text = (playerStats[selected].experienceToLevel[playerStats[selected].playerLevel] - playerStats[selected].playerExperience).ToString();
        statusImage.sprite = playerStats[selected].charImage;
    }

    // Save game from menu button
    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }
    public void LoadGame()
    {
        GameManager.instance.LoadData();
        QuestManager.instance.LoadQuestData();
    }

    // Quit game and load back into main menu
    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(gameObject);
    }
}
