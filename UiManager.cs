using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private UiData playerUiData;
    [SerializeField] private Button atkButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private GameObject draftPanel;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text incomeText;
    [SerializeField] private TMP_Text spinsLeftText;
    [SerializeField] private TMP_Text rerollText;
    [SerializeField] private TMP_Text itemRerollText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text removeButtonText;
    [SerializeField] private TMP_Text shopUpgradeText;
    [SerializeField] private TMP_Text notificationText;

    private bool removeMode;

    public GameObject[] draftCards = new GameObject[5];
    public GameObject[] itemCards = new GameObject[3];
    public int rollCost;
    public int itemRollCost;
    public int removeCost;
    public int draftInitialCost;
    public int draftRarityCost;
    public int shopUpgradePrice;

    private void Awake()
    {
        GameEventManager.instance.draftPhase += StartDraft;
        GameEventManager.instance.clearUnits += ToggleButton;
        GameEventManager.instance.draftPhase += ToggleButton;
        GameEventManager.instance.endPhase += EndPhase;
        GameEventManager.instance.clearUnits += ClearPhase;
        GameEventManager.instance.newWave += RestockSpins;
        GameEventManager.instance.removeUnitModeOn += RemoveModeOn;
        GameEventManager.instance.removeUnitModeOff += RemoveModeOff;
        GameEventManager.instance.itemPhase += ResetItemPrices;
        GameEventManager.instance.itemBought += UpdateItemPrices;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEventManager.instance.NewGame();
        playerUiData.wave = 1;
        playerUiData.spinsLeft = playerUiData.startSpins;
        playerUiData.gold = 0;
        playerUiData.incomeTier = 0;

        itemPanel.SetActive(false);
        draftPanel.SetActive(false);
        loseScreen.SetActive(false);
        startScreen.SetActive(true);

        atkButton.interactable = true;
        removeMode = false;

        rerollText.SetText("Reroll: " + rollCost.ToString());
        itemRerollText.SetText("Reroll: " + itemRollCost.ToString());
        removeButtonText.SetText("Remove: " + removeCost.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        goldText.SetText("Gold: " + playerUiData.gold);
        spinsLeftText.SetText("Time: " + playerUiData.spinsLeft);
        waveText.SetText("Wave " + playerUiData.wave);
        incomeText.SetText("Income: 2 + " + playerUiData.incomeTier);
        shopUpgradeText.SetText("Upgrade Shop " + shopUpgradePrice);
        
        playerUiData.UpdateIncomeTier();

        if (playerUiData.gold >= removeCost && GameAssets.i.unitManager.GetPhase() == 4)
        {
            removeButton.interactable = true;
        }
        else
        {
            removeButton.interactable = false;
        }

        if (!removeMode)
        {
            removeButtonText.SetText("Remove: " + removeCost.ToString());
        }
        else
        {
            removeButtonText.SetText("Cancel");
        }
    }

    public void StartDraft()
    {
        if (playerUiData.spinsLeft <= 0 && GameAssets.i.unitManager.GetEnemiesOnBoard().Count > 0)
        {
            loseScreen.SetActive(true);
        }
        else if (GameAssets.i.unitManager.GetEnemiesOnBoard().Count <= 0)
        {
            GameEventManager.instance.ItemPhase();
            SetNotificationText("");
            itemPanel.SetActive(true);
            RollItem();
            GameEventManager.instance.NewWave();
        }
        else
        {
            draftPanel.SetActive(true);
            RollDraft();
        }
    }

    public void RollDraft()
    {
        foreach (GameObject draft in draftCards)
        {
            UnitSObject randUnit = GameAssets.i.unitManager.UnitList()[Random.Range(0, GameAssets.i.unitManager.UnitList().Count)];
            draft.GetComponent<DraftCard>().SetUnit(randUnit);
            draft.GetComponent<DraftCard>().SetDisplay();
            draft.SetActive(true);
        }
    }

    public void RollItem()
    {
        foreach (GameObject draft in itemCards)
        {
            ItemObject randItem = GameAssets.i.itemManager.ItemsToSale()[Random.Range(0, GameAssets.i.itemManager.ItemsToSale().Count)];
            draft.GetComponent<ItemDraft>().SetItem(randItem);
            draft.GetComponent<ItemDraft>().SetDisplay();
            draft.SetActive(true);
        }
    }

    public void RerollDraft()
    {
        if (playerUiData.gold >= rollCost)
        {
            RollDraft();
            playerUiData.gold -= rollCost;
        }
    }

    public void RerollItems()
    {
        if (playerUiData.gold >= itemRollCost)
        {
            RollItem();
            playerUiData.gold -= itemRollCost;
        }
    }

    public void ShopUpgrade()
    {
        if (playerUiData.gold >= shopUpgradePrice)
        {
            GameAssets.i.unitManager.UpgradeShop();
            playerUiData.gold -= shopUpgradePrice;
            shopUpgradePrice++;
        }

    }

    public void RestockSpins()
    {
        playerUiData.wave++;
        playerUiData.spinsLeft += playerUiData.startSpins;
    }

    public void ActivateDraftPanel()
    {
        draftPanel.SetActive(true);
    }

    public void DisableDraftPanel()
    {
        draftPanel.SetActive(false);
    }

    public void DisableItemPanel()
    {
        itemPanel.SetActive(false);
    }

    public void ToggleButton()
    {
        atkButton.interactable = !atkButton.interactable ;
    }

    public void EndPhase()
    {
        playerUiData.GenerateIncome();
    }

    public void ClearPhase()
    {
        playerUiData.spinsLeft--;
    }

    public UiData GetPlayerUiData()
    {
        return playerUiData;
    }

    public void TryAgainButton()
    {
        GameEventManager.instance.NewGame();

        playerUiData.wave = 1;
        playerUiData.spinsLeft = playerUiData.startSpins;
        playerUiData.gold = 0;
        playerUiData.incomeTier = 0;
        playerUiData.maxIncomeTier = 3;

        draftPanel.SetActive(false);
        loseScreen.SetActive(false);

        atkButton.interactable = true;

        rerollText.SetText("Reroll: " + rollCost.ToString());
    }

    public void RemoveButton()
    {
        if (removeMode)
        {
            GameEventManager.instance.RemoveUnitModeOff();
        }
        else
        {
            GameEventManager.instance.RemoveUnitModeOn();
        }
    }

    public void DisableStartScreen()
    {
        startScreen.SetActive(false);
    }

    public void RemoveModeOn()
    {
        removeMode = true;
    }

    public void RemoveModeOff()
    {
        removeMode = false;
    }

    public void ResetItemPrices()
    {
        foreach (var item in itemCards)
        {
            item.GetComponent<ItemDraft>().ResetCost();
        }
    }

    public void UpdateItemPrices()
    {
        foreach (var item in itemCards)
        {
            item.GetComponent<ItemDraft>().IncreaseCost();
        }
    }

    public void SetNotificationText(string text)
    {
        notificationText.SetText(text);
    }
}
