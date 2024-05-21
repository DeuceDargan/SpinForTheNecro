using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "ScriptableObject/Unit")]

public class UnitSObject : ScriptableObject
{
    private UnitDisplay display;
    private Vector2 pos;

    public enum UnitRace
    {
        ENEMY, BONEMEN, ZOMBIE, ANCIENT, TOKEN
    };

    public enum UnitRarity
    {
        COMMON, RARE, LEGENDARY
    };

    public enum EffectType
    {
        ONSPIN, ONHIT, ENDTURN, ONDEATH, ONREMOVE, ONDMG
    };

    [Header("Display")]
    public string unitName;
    public UnitRace race;
    public string description;
    public UnitRarity rarity;
    public Sprite sprite;
    public bool isEnemy;

    [Header("Effect")]
    public UnitEffects spinEffect;
    public int spinEffectiveness;
    public UnitEffects endEffect;
    public int endEffectiveness;
    public UnitEffects onHitEffect;
    public int onHitEffectiveness;
    public UnitEffects onDeathEffect;
    public int onDeathEffectiveness;
    public UnitEffects onRemoveEffect;
    public int onRemoveEffectiveness;
    public UnitEffects onDmgEffect;
    public int onDmgEffectiveness;

    [Header("Health")]
    public int baseHealth;
    public int maxHealth;
    public int health;
    public bool hasHealth;

    [Header("Attack")]
    public int baseAtk;
    public int unitAtk;
    public bool isRanged;
    public bool isCombo;
    public bool isAoe;
    public GameObject projectile;
    

    [Header("Other")]
    public bool hitThisSpin;
    public int tempAtk;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void ActivateEffect(int type)
    {
        switch ((EffectType)type)
        {
            case EffectType.ONSPIN:
                spinEffect?.ExecuteEffect(spinEffectiveness, this);
                break;

            case EffectType.ONHIT:
                onHitEffect?.ExecuteEffect(onHitEffectiveness, this);
                break;

            case EffectType.ENDTURN:
                endEffect?.ExecuteEffect(endEffectiveness, this);
                break;

            case EffectType.ONDEATH:
                onDeathEffect?.ExecuteEffect(onDeathEffectiveness, this);
                break;

            case EffectType.ONREMOVE:
                onRemoveEffect?.ExecuteEffect(onRemoveEffectiveness, this);
                break;

            case EffectType.ONDMG:
                onDmgEffect?.ExecuteEffect(onDmgEffectiveness, this);
                break;
        }
    }

    public void SetDisplay(UnitDisplay d)
    {
        display = d;
    }

    public void SetPos (Vector2 v)
    {
        pos = v;
    }

    public void scaleUnit()
    {
        UiData ui = GameAssets.i.uiManager.GetPlayerUiData();
        float scaledHealth = maxHealth * ui.healthScaler;
        maxHealth = (int)scaledHealth;
    }

    public UnitDisplay GetUnitDisplay()
    {
        return display;
    }

    public void NewSpin()
    {
        hitThisSpin = false;
        tempAtk = 0;
    }
}