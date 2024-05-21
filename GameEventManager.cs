using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<GameObject> selectGameObject;

    public void SelectGameObject(GameObject gObject)
    {
        if (selectGameObject != null)
        {
            selectGameObject(gObject);
            Debug.Log("Select " + gObject);
        }
    }

    public event Action spawnEnemyUnits;
    public void SpawnEnemyUnits()
    {
        if (spawnEnemyUnits != null)
        {
            spawnEnemyUnits();
            Debug.Log("spawnEnemyUnits");
        }
    }

    public event Action spawnPlayerUnits;
    public void SpawnPlayerUnits()
    {
        if (spawnPlayerUnits != null)
        {
            spawnPlayerUnits();
            Debug.Log("spawnPlayerUnits");
        }
    }

    public event Action unitsActions;
    public void UnitsActions()
    {
        if (unitsActions != null)
        {
            unitsActions();
            Debug.Log("unitsActions");
        }
    }

    public event Action enemyActions;
    public void EnemyActions()
    {
        if (enemyActions != null)
        {
            enemyActions();
            Debug.Log("enemyActions");
        }
    }

    public event Action clearUnits;
    public void ClearUnits()
    {
        if (clearUnits != null)
        {
            clearUnits();
            Debug.Log("clearUnits");
        }
    }

    public event Action endPhase;
    public void EndPhase()
    {
        if (endPhase != null)
        {
            endPhase();
            Debug.Log("endPhase");
        }
    }

    public event Action draftPhase;
    public void DraftPhase()
    {
        if (draftPhase != null)
        {
            draftPhase();
            Debug.Log("draftPhase");
        }
    }

    public event Action removeKillUnits;
    public void RemoveKillUnits()
    {
        if (removeKillUnits != null)
        {
            removeKillUnits();
            Debug.Log("RemoveKillUnits");
        }
    }

    public event Action newGame;
    public void NewGame()
    {
        if (newGame != null)
        {
            newGame();
            Debug.Log("newGameStart");
        }
    }

    public event Action newWave;
    public void NewWave()
    {
        if (newWave != null)
        {
            newWave();
            Debug.Log("newWave");
        }
    }

    public event Action removeUnitModeOn;
    public void RemoveUnitModeOn()
    {
        if (removeUnitModeOn != null)
        {
            removeUnitModeOn();
            Debug.Log("removeUnitModeOn");
        }
    }

    public event Action removeUnitModeOff;
    public void RemoveUnitModeOff()
    {
        if (removeUnitModeOff != null)
        {
            removeUnitModeOff();
            Debug.Log("removeUnitModeOff");
        }
    }

    public event Action itemPhase;
    public void ItemPhase()
    {
        if (itemPhase != null)
        {
            itemPhase();
            Debug.Log("itemPhase");
        }
    }

    public event Action itemBought;
    public void ItemBought()
    {
        if (itemBought != null)
        {
            itemBought();
            Debug.Log("itemBought");
        }
    }
}
