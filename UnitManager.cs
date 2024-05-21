using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public enum Phase
    {
        CLEAR, PLACEUNITS, PLAYERTURN, ENEMYTURN, END
    };

    private Phase inPhase;

    private List<UnitDisplay> unitsOnBoard = new List<UnitDisplay>();
    private List<UnitDisplay> enemiesOnBoard = new List<UnitDisplay>();
    private List<Vector2> tiles = new List<Vector2>();
    private List<Vector2> tilesToUse = new List<Vector2>();

    public List<WaveOfEnemies> waves = new List<WaveOfEnemies>();
    public List<UnitSObject> units = new List<UnitSObject>();
    public List<UnitSObject> unitsOnHold = new List<UnitSObject>();
    public List<UnitSObject> enemies = new List<UnitSObject>();
    public List<UnitSObject> hand = new List<UnitSObject>();
    public List<UnitSObject> enemyHand = new List<UnitSObject>();

    public float atkInterval;
    public float effectInterval;
    public float textInterval;
    public float phaseDelay;

    private void Awake()
    {
        GameEventManager.instance.spawnPlayerUnits += PlacePlayerUnits;
        GameEventManager.instance.spawnEnemyUnits += PlaceEnemyUnits;
        GameEventManager.instance.clearUnits += ClearUnits;
        GameEventManager.instance.unitsActions += UnitsActions;
        GameEventManager.instance.enemyActions += EnemyActions;
        GameEventManager.instance.endPhase += UnitEndPhase;
        GameEventManager.instance.newGame += NewBoard;
        GameEventManager.instance.newWave += NextWave;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (UnitSObject item in enemies)
        {
            item.maxHealth = item.baseHealth;
            item.unitAtk = item.baseAtk;
        }

        foreach (UnitSObject item in unitsOnHold)
        {
            item.maxHealth = item.baseHealth;
            item.unitAtk = item.baseAtk;
        }

        foreach (UnitSObject item in units)
        {
            item.maxHealth = item.baseHealth;
            item.unitAtk = item.baseAtk;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewBoard()
    {
        inPhase = Phase.END;

        tilesToUse = new List<Vector2>(tiles);

        unitsOnBoard = new List<UnitDisplay>();
        enemiesOnBoard = new List<UnitDisplay>();

        hand = new List<UnitSObject>();
        enemyHand = new List<UnitSObject>();

        AddToHand(units[Random.Range(0,4)]);
        AddToHand(units[Random.Range(0,4)]);
        AddToHand(units[Random.Range(0,4)]);
        AddToHand(units[Random.Range(0,4)]);

        AddToEnemy(enemies[0]);
        AddToEnemy(enemies[0]);
        AddToEnemy(enemies[0]);
        AddToEnemy(enemies[0]);

        while(tilesToUse.Count != 0 && enemyHand.Count != 0)
        {
            UnitSObject randUnit = enemyHand[Random.Range(0, enemyHand.Count)];
            Vector2 tilePos = tilesToUse[Random.Range(0, tilesToUse.Count)];
            Tile randTile = GameAssets.i.board.GetTileAtPos(tilePos);
            enemyHand.Remove(randUnit);
            tilesToUse.Remove(tilePos); 
            
            UnitDisplay boardPiece = Instantiate(GameAssets.i.unitPiece, randTile.transform.position, Quaternion.identity).GetComponent<UnitDisplay>();
            boardPiece.SetUnit(randUnit);
            enemiesOnBoard.Add(boardPiece);
            randTile.SetUnit(boardPiece);
            boardPiece.SetPos(tilePos);
            randUnit.SetDisplay(boardPiece);
        }
    }

    public void AttackButton()
    {
        if (inPhase == Phase.END)
        {
            GameEventManager.instance.RemoveUnitModeOff();
            GameEventManager.instance.ClearUnits();
        }
    }

    public void ClearUnits()
    {
        foreach (var item in unitsOnBoard)
        {
            item.GetUnit().NewSpin();
        }

        inPhase = Phase.CLEAR;
        StartCoroutine(ClearUnitsOnBoard());
    }

    private IEnumerator ClearUnitsOnBoard()
    {
        tilesToUse = new List<Vector2>(tiles);

        foreach(var unit in unitsOnBoard)
        {
            unit.ClearUnit();
        }
        unitsOnBoard = new List<UnitDisplay>();

        foreach(var unit in enemiesOnBoard)
        {
            unit.ClearUnit();
        }
        enemiesOnBoard = new List<UnitDisplay>();

        yield return new WaitForSeconds(phaseDelay);

        GameEventManager.instance.SpawnEnemyUnits();
    }

    public void PlaceEnemyUnits()
    {
        inPhase = Phase.PLACEUNITS;
        StartCoroutine(PlaceEnemiesOnBoard());
    }

    private IEnumerator PlaceEnemiesOnBoard()
    {
        while(tilesToUse.Count != 0 && enemyHand.Count != 0)
        {
            UnitSObject randUnit = enemyHand[Random.Range(0, enemyHand.Count)];
            Vector2 tilePos = tilesToUse[Random.Range(0, tilesToUse.Count)];
            Tile randTile = GameAssets.i.board.GetTileAtPos(tilePos);
            enemyHand.Remove(randUnit);
            tilesToUse.Remove(tilePos); 
            
            UnitDisplay boardPiece = Instantiate(GameAssets.i.unitPiece, randTile.transform.position, Quaternion.identity).GetComponent<UnitDisplay>();
            boardPiece.SetUnit(randUnit);
            enemiesOnBoard.Add(boardPiece);
            randTile.SetUnit(boardPiece);
            boardPiece.SetPos(tilePos);
            randUnit.SetDisplay(boardPiece);
        }

        yield return new WaitForSeconds(phaseDelay);

        GameEventManager.instance.SpawnPlayerUnits();
    }

    public void PlacePlayerUnits()
    {
        StartCoroutine(PlaceUnitsOnBoard());
    }

    private IEnumerator PlaceUnitsOnBoard()
    {
        while(tilesToUse.Count != 0 && hand.Count != 0)
        {
            UnitSObject randUnit = hand[Random.Range(0, hand.Count)];
            Vector2 tilePos = tilesToUse[Random.Range(0, tilesToUse.Count)];
            Tile randTile = GameAssets.i.board.GetTileAtPos(tilePos);
            hand.Remove(randUnit);
            tilesToUse.Remove(tilePos); 

            UnitDisplay boardPiece = Instantiate(GameAssets.i.unitPiece, randTile.transform.position, Quaternion.identity).GetComponent<UnitDisplay>();
            boardPiece.SetUnit(randUnit);
            unitsOnBoard.Add(boardPiece);
            randTile.SetUnit(boardPiece);
            boardPiece.SetPos(tilePos);
            randUnit.SetDisplay(boardPiece);
        }

        yield return new WaitForSeconds(phaseDelay);

        GameEventManager.instance.UnitsActions();
    }

    public void UnitsActions()
    {
        inPhase = Phase.PLAYERTURN;
        StartCoroutine(UnitsActionsPlay());
    }

    private IEnumerator UnitsActionsPlay()
    {
        foreach (var unit in unitsOnBoard)
        { 
            if (unit.GetUnit().spinEffect != null)
            {
                unit.GetUnit().ActivateEffect(0);
                yield return new WaitForSeconds(effectInterval);
            }  
        }

        foreach (var unit in unitsOnBoard)
        {
            List<UnitDisplay> inRange = new List<UnitDisplay>(unit.GetAoe());

            if ((unit.GetUnit().isRanged || inRange.Count > 0) && unit.GetUnit().unitAtk > 0)
            {
                unit.PlayAnim("UnitAttack");
                yield return new WaitForSeconds(atkInterval);

                if (unit.GetUnit().isCombo)
                {
                    unit.PlayAnim("UnitAttack");
                    yield return new WaitForSeconds(atkInterval);
                }
            }
        }

        yield return new WaitForSeconds(phaseDelay);
        GameEventManager.instance.RemoveKillUnits();
        GameEventManager.instance.EnemyActions();
    }

    public void EnemyActions()
    {
        inPhase = Phase.ENEMYTURN;
        StartCoroutine(EnemyActionsPlay());
    }

    private IEnumerator EnemyActionsPlay()
    {
        foreach (var unit in enemiesOnBoard)
        {
            List<UnitDisplay> inRange = new List<UnitDisplay>(unit.GetAoe());

            if (unit.GetUnit().isRanged || inRange.Count > 0)
            {
                unit.PlayAnim("UnitAttack");
                yield return new WaitForSeconds(atkInterval);

                if (unit.GetUnit().isCombo)
                {
                    unit.PlayAnim("UnitAttack");
                    yield return new WaitForSeconds(atkInterval);
                }
            }
        }

        foreach (var unit in enemiesOnBoard)
        { 
            if (unit.GetUnit().spinEffect != null)
            {
                unit.GetUnit().ActivateEffect(0);
                yield return new WaitForSeconds(effectInterval);
            }  
        }

        yield return new WaitForSeconds(phaseDelay);
        GameEventManager.instance.RemoveKillUnits();
        GameEventManager.instance.EndPhase();
    }

    public void UnitEndPhase()
    {
        inPhase = Phase.END;
        StartCoroutine(UnitEndEffectAnim());
    }

    private IEnumerator UnitEndEffectAnim()
    {
        for (int x = 0; x < unitsOnBoard.Count; x++)
        {
            if (unitsOnBoard[x].gameObject.activeSelf && unitsOnBoard[x].GetUnit().endEffect != null)
            {
                unitsOnBoard[x].GetUnit().ActivateEffect(2);
                yield return new WaitForSeconds(effectInterval);
            } 
        }

        // foreach (var unit in unitsOnBoard)
        // {           
        //     if (unit.gameObject.activeSelf && unit.GetUnit().endEffect != null)
        //     {
        //         unit.GetUnit().ActivateEffect(2);
        //         yield return new WaitForSeconds(effectInterval);
        //     }  
        // }

        foreach (var unit in enemiesOnBoard)
        {
            if (unit.gameObject.activeSelf && unit.GetUnit().endEffect != null)
            {
                unit.GetUnit().ActivateEffect(2);
                yield return new WaitForSeconds(effectInterval);
            }
        }

        yield return new WaitForSeconds(phaseDelay);
        GameEventManager.instance.RemoveKillUnits();
        GameEventManager.instance.DraftPhase();
    }

    public void NextWave()
    {
        int enemiesInWave = 4 + (GameAssets.i.uiManager.GetPlayerUiData().wave / 10);
        int waveNum = GameAssets.i.uiManager.GetPlayerUiData().wave;

        foreach (UnitSObject item in enemies)
        {
            item.scaleUnit();
        }

        inPhase = Phase.END;

        foreach(var unit in unitsOnBoard)
        {
            unit.ClearUnit();
        }

        unitsOnBoard = new List<UnitDisplay>();

        tilesToUse = new List<Vector2>(tiles);

        enemiesOnBoard = new List<UnitDisplay>();

        enemyHand = new List<UnitSObject>();


        for (int x = 0; x < enemiesInWave; x++)
        {
            AddToEnemy(enemies[Random.Range(0,enemies.Count)]);
        }

        while(tilesToUse.Count != 0 && enemyHand.Count != 0)
        {
            UnitSObject randUnit = enemyHand[Random.Range(0, enemyHand.Count)];
            Vector2 tilePos = tilesToUse[Random.Range(0, tilesToUse.Count)];
            Tile randTile = GameAssets.i.board.GetTileAtPos(tilePos);
            enemyHand.Remove(randUnit);
            tilesToUse.Remove(tilePos); 
            
            UnitDisplay boardPiece = Instantiate(GameAssets.i.unitPiece, randTile.transform.position, Quaternion.identity).GetComponent<UnitDisplay>();
            boardPiece.SetUnit(randUnit);
            enemiesOnBoard.Add(boardPiece);
            randTile.SetUnit(boardPiece);
            boardPiece.SetPos(tilePos);
            randUnit.SetDisplay(boardPiece);
        }
    }

    public void SetBoard()
    {
        foreach(var tile in GameAssets.i.board.GetDictionary().Keys)
        {
            tiles.Add(tile);
        }

        tilesToUse = new List<Vector2>(tiles);

        NewBoard();
    }

    public void AddToHand(UnitSObject unit)
    {
        hand.Add(Instantiate(unit));
    }

    public void AddToEnemy(UnitSObject unit)
    {
        UnitSObject newUnit = Instantiate(unit);
        enemyHand.Add(newUnit);
    }

    public List<UnitSObject> GetHand()
    {
        return hand;
    }

    public List<UnitDisplay> GetEnemiesOnBoard()
    {
        return enemiesOnBoard;
    }

    public List<UnitDisplay> GetUnitsOnBoard()
    {
        return unitsOnBoard;
    }

    public List<UnitSObject> UnitList()
    {
        return units;
    }

    public List<UnitSObject> UnitsOnHold()
    {
        return unitsOnHold;
    }

    public int GetPhase()
    {
        return (int)inPhase;
    }

    public void RemoveUnitFromBoard (UnitDisplay unit)
    {
        if (unit.GetUnit().isEnemy)
        {
            enemiesOnBoard.Remove(unit);
        }
        else
        {
            unitsOnBoard.Remove(unit); 
        }
    }

    public void UpgradeShop()
    {
        if (unitsOnHold != null)
        {
            UnitSObject newUnit = unitsOnHold[Random.Range(0, unitsOnHold.Count)];
            units.Add(newUnit);
            unitsOnHold.Remove(newUnit);
            GameAssets.i.uiManager.SetNotificationText(newUnit.unitName + " has been added to the roster");
        }
    }
}
