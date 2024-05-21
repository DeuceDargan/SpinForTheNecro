using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "ScriptableObject/UnitEffect")]
public class UnitEffects : ScriptableObject
{
    public enum Effect
    {
        ADDUNIT, UNITTOGOLD, ADDGOLD, HEALRACE, HEALIFHIT, TRANSFORMSELF, TRANSFORMOTHER, UNITTOATK, UNITTOHEALTH,
        DIE, GOLDCHANCE, GACHACHANCE, ADDATKNEARCLASS, HEALRAND, ATKPERGOLD, BUFFRAND
    };

    public Effect effect;
    public UnitSObject unit;
    public UnitSObject unit2;
    public int race;
    int dice;

    public void ExecuteEffect(int effectiveness, UnitSObject user)
    {
        List<UnitDisplay> aoe = new List<UnitDisplay>(user.GetUnitDisplay().GetAoeAllies());
        List<UnitDisplay> onBoard = GameAssets.i.unitManager.GetUnitsOnBoard();

        user.GetUnitDisplay().PlayAnim("UnitEffect");

        switch (effect)
        {   
            case Effect.ADDUNIT:
                for (int x = 0; x < effectiveness; x++)
                {
                    GameAssets.i.unitManager.AddToHand(Instantiate(unit));
                }
                break;

            case Effect.UNITTOGOLD:
                foreach (UnitDisplay u in aoe)
                {
                    if (u.GetUnit().unitName == unit.unitName)
                    {
                        u.RemoveUnit();
                        GameAssets.i.uiManager.GetPlayerUiData().gold += effectiveness;
                    }
                }
                break;

            case Effect.HEALRACE:
                foreach (UnitDisplay u in aoe)
                {
                    if ((int)u.GetUnit().race == race)
                    {
                        u.GetUnit().health += effectiveness;
                        u.PlayAnim("UnitHeal");
                    }
                }
                break;

            case Effect.ADDGOLD:
                GameAssets.i.uiManager.GetPlayerUiData().gold += effectiveness;
                break;

            case Effect.HEALIFHIT:
                if (!user.hitThisSpin)
                {
                    user.health += effectiveness;
                    user.GetUnitDisplay().PlayAnim("UnitHeal");
                }
                break;

            case Effect.TRANSFORMSELF:
                user.GetUnitDisplay().PlayAnim("UnitTransform");
                user = Instantiate(unit);
                break;

            case Effect.TRANSFORMOTHER:
                foreach (UnitDisplay u in aoe)
                {
                    if (u.GetUnit().unitName == unit.unitName)
                    {
                        u.PlayAnim("UnitTransform");
                        u.SetUnit(Instantiate(unit2));
                    }
                }
                break;

            case Effect.UNITTOATK:
                foreach (UnitDisplay u in aoe)
                {
                    if (u.GetUnit().unitName == unit.unitName)
                    {
                        u.RemoveUnit();
                        user.unitAtk += effectiveness;
                        u.PlayAnim("UnitBuff");
                    }
                }
                break;

            case Effect.UNITTOHEALTH:
                foreach (UnitDisplay u in aoe)
                {
                    if (u.GetUnit().unitName == unit.unitName)
                    {
                        u.RemoveUnit();
                        user.health += effectiveness;
                        u.PlayAnim("UnitHeal");
                    }
                }
                break;

            case Effect.DIE:
                user.GetUnitDisplay().ExecuteUnit(); 
                break;

            case Effect.GOLDCHANCE:
                dice = Random.Range(0, 4);

                if (dice == 1)
                {
                    GameAssets.i.uiManager.GetPlayerUiData().gold += effectiveness;
                }
                break;

            case Effect.GACHACHANCE:

                dice = Random.Range(0, 100000);

                if (dice == 1)
                {
                    GameAssets.i.uiManager.GetPlayerUiData().gold += effectiveness;
                }
                break;

            case Effect.ADDATKNEARCLASS:
                
                foreach (UnitDisplay item in aoe)
                {
                    if ((int)item.GetUnit().race == race)
                    {
                        user.tempAtk += effectiveness;
                        if (user.tempAtk > 0)
                        {
                            user.GetUnitDisplay().PlayAnim("UnitBuff");
                        }
                    }
                }
                break;

            case Effect.HEALRAND:
                UnitSObject temp01 = onBoard[Random.Range(0,onBoard.Count)].GetUnit();
                temp01.health += effectiveness;
                temp01.GetUnitDisplay().PlayAnim("UnitHeal");
                break;

            case Effect.ATKPERGOLD:
                user.tempAtk += GameAssets.i.uiManager.GetPlayerUiData().gold%effectiveness;
                if (user.tempAtk > 0)
                {
                    user.GetUnitDisplay().PlayAnim("UnitBuff");
                }
                break;

            case Effect.BUFFRAND:
                UnitSObject temp02 = onBoard[Random.Range(0,onBoard.Count)].GetUnit();
                temp02.unitAtk += effectiveness;
                temp02.GetUnitDisplay().PlayAnim("UnitBuff");
                break;
        }
    }
}