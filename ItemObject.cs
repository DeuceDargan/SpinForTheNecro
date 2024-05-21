using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObject/Item")]

public class ItemObject : ScriptableObject
{
    public enum Item
    {
        WHETSTONE, GATORADE, CHAINMAIL, LEATHER, GRINDSTONE, BRAINDISH, BOOKOFA, FINANCE
    };

    public string itemName;
    public string description;
    public Sprite sprite;
    public Item item;
    public bool isUnique;

    public void ActicvateItem()
    {
        List<UnitSObject> units = GameAssets.i.unitManager.UnitList();
        List<UnitSObject> unitsOnHold = GameAssets.i.unitManager.UnitsOnHold();
        List<UnitDisplay> unitsOnBoard = GameAssets.i.unitManager.GetUnitsOnBoard();

        switch (item)
        {
            case Item.WHETSTONE:
    
                foreach (UnitSObject item in units)
                {
                    if (!item.isRanged)
                    {
                        item.unitAtk++;
                    }
                }

                foreach (UnitSObject item in unitsOnHold)
                {
                    if (!item.isRanged)
                    {
                        item.unitAtk++;
                    }
                }

                foreach (UnitDisplay item in unitsOnBoard)
                {
                    UnitSObject u = item.GetUnit();

                    if (!u.isRanged)
                    {
                        u.unitAtk++;
                    }
                }
                break;

            case Item.GATORADE:
    
                foreach (UnitSObject item in units)
                {
                    if (item.isRanged)
                    {
                        item.unitAtk += 2;
                    }
                }

                foreach (UnitSObject item in unitsOnHold)
                {
                    if (item.isRanged)
                    {
                        item.unitAtk += 2;
                    }
                }

                foreach (UnitDisplay item in unitsOnBoard)
                {
                    UnitSObject u = item.GetUnit();

                    if (u.isRanged)
                    {
                        u.unitAtk += 2;
                    }
                }
                break;

            case Item.CHAINMAIL:
    
                foreach (UnitSObject item in units)
                {
                    if (!item.isRanged)
                    {
                        item.maxHealth += 2;
                        item.health += 2;
                    }
                }

                foreach (UnitSObject item in unitsOnHold)
                {
                    if (!item.isRanged)
                    {
                        item.maxHealth += 2;
                        item.health += 2;
                    }
                }

                foreach (UnitDisplay item in unitsOnBoard)
                {
                    UnitSObject u = item.GetUnit();

                    if (!u.isRanged)
                    {
                        u.maxHealth += 2;
                        u.health += 2;
                    }
                }
                break;

            case Item.LEATHER:
    
                foreach (UnitSObject item in units)
                {
                    if (item.isRanged)
                    {
                        item.maxHealth++;
                        item.health++;
                    }
                }

                foreach (UnitSObject item in unitsOnHold)
                {
                    if (item.isRanged)
                    {
                        item.maxHealth++;
                        item.health++;
                    }
                }

                foreach (UnitDisplay item in unitsOnBoard)
                {
                    UnitSObject u = item.GetUnit();

                    if (u.isRanged)
                    {
                        u.maxHealth++;
                        u.health++;
                    }
                }
                break;

            case Item.GRINDSTONE:

                foreach (UnitSObject item in units)
                {
                    if ((int)item.race == 1)
                    {
                        item.unitAtk++;
                    }
                }

                foreach (UnitSObject item in unitsOnHold)
                {
                    if ((int)item.race == 1)
                    {
                        item.unitAtk++;
                    }
                }

                foreach (UnitDisplay item in unitsOnBoard)
                {
                    UnitSObject u = item.GetUnit();

                    if ((int)u.race == 1)
                    {
                        u.unitAtk++;
                    }
                }
                break;

            case Item.BRAINDISH:

                foreach (UnitSObject item in units)
                {
                    if ((int)item.race == 2)
                    {
                        item.maxHealth++;
                        item.health++;
                    }
                }

                foreach (UnitSObject item in unitsOnHold)
                {
                    if ((int)item.race == 2)
                    {
                        item.maxHealth++;
                        item.health++;
                    }
                }

                foreach (UnitDisplay item in unitsOnBoard)
                {
                    UnitSObject u = item.GetUnit();

                    if ((int)u.race == 2)
                    {
                        u.maxHealth++;
                        u.health++;
                    }
                }
                break;

            case Item.BOOKOFA:

                foreach (UnitSObject item in units)
                {
                    if ((int)item.race == 3)
                    {
                        item.unitAtk++;
                        item.maxHealth++;
                        item.health++;
                    }
                }

                foreach (UnitSObject item in unitsOnHold)
                {
                    if ((int)item.race == 3)
                    {
                        item.unitAtk++;
                        item.maxHealth++;
                        item.health++;
                    }
                }

                foreach (UnitDisplay item in unitsOnBoard)
                {
                    UnitSObject u = item.GetUnit();

                    if ((int)u.race == 3)
                    {
                        u.unitAtk++;
                        u.maxHealth++;
                        u.health++;
                    }
                }
                break;
        
            case Item.FINANCE:

                GameAssets.i.uiManager.GetPlayerUiData().maxIncomeTier++;
                break;
        }
    }
}
