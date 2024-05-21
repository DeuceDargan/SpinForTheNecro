using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DraftCard : MonoBehaviour
{
    private int price;
    [SerializeField] private UnitSObject unit;
    [SerializeField] private Image image;
    [SerializeField] private Image atkType;
    [SerializeField] private Sprite meeleType;
    [SerializeField] private Sprite rangeType;
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text heatlhText;
    [SerializeField] private TMP_Text typeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToHand()
    {
        UiData player = GameAssets.i.uiManager.GetPlayerUiData();

        if (player.gold >= price)
        {
            GameAssets.i.unitManager.GetHand().Add(Instantiate(unit));
            gameObject.SetActive(false);
            player.gold -= price;
        }
    }

    public void SetUnit(UnitSObject u)
    {
        unit = u;
    }

    public void SetDisplay()
    {
        if (unit != null)
        {
            image.sprite = unit.sprite;
            unitName.SetText(unit.unitName);
            description.SetText(unit.description);
            price = GameAssets.i.uiManager.draftInitialCost + (GameAssets.i.uiManager.draftRarityCost * (int)unit.rarity);
            cost.SetText("Gold: " + price.ToString());
            atkText.SetText(unit.unitAtk.ToString());
            heatlhText.SetText(unit.maxHealth.ToString());

            if (unit.isRanged)
            {
                atkType.sprite = rangeType;
            }
            else
            {
                atkType.sprite = meeleType;
            }

            switch((int)unit.race)
            {
                case 1:
                    typeText.SetText("Boneman");
                    break;

                case 2:
                    typeText.SetText("Rotman");
                    break;

                case 3:
                    typeText.SetText("Ancient");
                    break;
            }
        }
    }
}