using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDraft : MonoBehaviour
{
    private ItemManager iManager;
    private UiData player;
    private int cost;
    
    [SerializeField] private ItemObject item;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text costText;

    // Start is called before the first frame update
    void Start()
    {
        iManager = GameAssets.i.itemManager;
        player = GameAssets.i.uiManager.GetPlayerUiData();
    }

    // Update is called once per frame
    void Update()
    {
        costText.SetText("Gold: " + cost.ToString());
    }

    public void AddToPouch()
    {
        if (player.gold >= cost)
        {
            item.ActicvateItem();

            if (item.isUnique)
            {
                iManager.RemoveFromSale(item);
            }

            iManager.AddToPouch(item);
            gameObject.SetActive(false);
            player.gold -= cost;
            GameEventManager.instance.ItemBought();
        }
    }

    public void SetItem(ItemObject i)
    {
        item = i;
    }

    public void SetDisplay()
    {
        if (item != null)
        {
            image.sprite = item.sprite;
            itemName.SetText(item.itemName);
            description.SetText(item.description);
        }
    }

    public void ResetCost()
    {
        cost = 0;
    }

    public void IncreaseCost()
    {
        if (cost == 0)
        {
            cost = 5;
        }
        else
        {
            cost++;
        }
    }
}