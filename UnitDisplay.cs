using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDisplay : MonoBehaviour
{
    private Vector2 tile;
    [SerializeField] private UnitSObject unit;
    [SerializeField] private SpriteRenderer theRenderer;
    [SerializeField] private Animator theAnim;
    [SerializeField] private TMP_Text healthText;


    private void Awake()
    {
        GameEventManager.instance.clearUnits += DestroyIfRemoved;
        GameEventManager.instance.removeKillUnits += KillUnit;
        GameEventManager.instance.newGame += Clean;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if (unit != null)
        {
            theRenderer.sprite = unit.sprite;

            if (unit.hasHealth)
            {
                if (unit.health > unit.maxHealth)
                {
                    unit.health = unit.maxHealth;
                }

                if (unit.health < 0)
                {
                    unit.health = 0;
                }

                healthText.SetText(unit.health.ToString());

                if (unit.isEnemy)
                {
                    healthText.color = Color.red;
                    theRenderer.flipX = true;
                }
            }
            else
            {
                healthText.SetText(" ");
            }
        }
    }

    public void Attack()
    {
        int totatAtk = unit.unitAtk + unit.tempAtk;

        if (totatAtk > 0)
        {
            if (unit.isRanged)
            {
                List<UnitDisplay> enemies;
                UnitDisplay enemy;
                enemies = new List<UnitDisplay>();

                if (unit.isEnemy)
                {
                    foreach (var e in GameAssets.i.unitManager.GetUnitsOnBoard())
                    {
                        if (e.GetUnit().hasHealth)
                        {
                            enemies.Add(e);
                        }
                    }
                }
                else
                {
                    foreach (var e in GameAssets.i.unitManager.GetEnemiesOnBoard())
                    {
                        if (e.GetUnit().hasHealth)
                        {
                            enemies.Add(e);
                        }
                    }
                }

                if (unit.isAoe)
                {
                    foreach (var e in enemies)
                    {
                        float angle = Mathf.Atan2(e.transform.position.y - transform.position.y,
                                                    e.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

                        GameObject p = Instantiate(unit.projectile,transform.position,Quaternion.Euler(0, 0, angle));
                        p.GetComponent<BoardProjectile>().target = e;
                        p.GetComponent<BoardProjectile>().dmg = totatAtk;
                    }
                }
                else
                {
                    enemy = enemies[Random.Range(0, enemies.Count)];

                    float angle = Mathf.Atan2(enemy.transform.position.y - transform.position.y,
                                            enemy.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

                    GameObject p = Instantiate(unit.projectile,transform.position,Quaternion.Euler(0, 0, angle));
                    p.GetComponent<BoardProjectile>().target = enemy;
                    p.GetComponent<BoardProjectile>().dmg = totatAtk;
                }
            }
            else
            {
                List<UnitDisplay> aoe = new List<UnitDisplay>(GetAoe());

            
                if (unit.isAoe)
                {
                    foreach (var target in aoe)
                    {
                        target.Hurt(totatAtk);
                    }
                }
                else
                {
                    if (aoe.Count > 0)
                    {
                        UnitDisplay target = aoe[Random.Range(0,aoe.Count)];
                        target.Hurt(totatAtk);
                    }
                }
            }
        }

        unit.ActivateEffect(1);
    }

    public void Hurt(int dmg)
    {
        unit.ActivateEffect(5);
        unit.health -= dmg;
        unit.hitThisSpin = true;
        PlayAnim("UnitDmg");
    }

    public List<UnitDisplay> GetAoe()
    {
        List<UnitDisplay> aoe = new List<UnitDisplay>();

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Tile t = GameAssets.i.board.GetTileAtPos(new Vector2(tile.x - 1 + x, tile.y - 1 + y));
                
                if (t != null && t.GetUnit() != null && t.GetUnit() != this && 
                    t.GetUnit().GetUnit().isEnemy != unit.isEnemy && t.GetUnit().GetUnit().hasHealth)
                {
                    aoe.Add(t.GetUnit());
                }
            }
        }

        return aoe;
    }

    public List<UnitDisplay> GetAoeAllies()
    {
        List<UnitDisplay> aoe = new List<UnitDisplay>();

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Tile t = GameAssets.i.board.GetTileAtPos(new Vector2(tile.x - 1 + x, tile.y - 1 + y));
                
                if (t != null && t.GetUnit() != null && t.GetUnit() != this &&
                    t.GetUnit().GetUnit().isEnemy == unit.isEnemy && t.GetUnit().GetUnit().health > 0)
                {
                    aoe.Add(t.GetUnit());
                }
            }
        }

        return aoe;
    }

    public void ClearUnit()
    {
        if (unit.health > 0 && gameObject.activeSelf)
        {
            if (unit.isEnemy)
            {
                GameAssets.i.unitManager.enemyHand.Add(unit);
            }
            else
            {
                GameAssets.i.unitManager.hand.Add(unit);
            }
        }

        Destroy(gameObject);
    }

    public void KillUnit()
    {
        if (unit.health <= 0)
        {
            if (gameObject.activeSelf)
            {
                unit.health = 0;
                gameObject.SetActive(false);
                unit.ActivateEffect(3);
                GameAssets.i.unitManager.RemoveUnitFromBoard(this);
            }
        }
    }

    public void ExecuteUnit()
    {
        unit.health = 0;
    }

    public void RemoveUnit()
    {
        if (gameObject.activeSelf)
        {
            unit.health = 0;
            gameObject.SetActive(false);
            GameAssets.i.unitManager.RemoveUnitFromBoard(this);
            unit.ActivateEffect(4);
        }
    }

    public void DestroyIfRemoved()
    {
        if (!gameObject.activeSelf)
        {
            Destroy(gameObject);
        }
    }

    public void PlayAnim(string anim)
    {
        if (gameObject.activeSelf)
        {
            theAnim.Play(anim);
        }
    }

    public void SetUnit(UnitSObject u)
    {
        unit = u;
    }

    public UnitSObject GetUnit()
    {
        return unit;
    }

    public void SetPos(Vector2 p)
    {
        tile = p;
    }

    public Vector2 GetTile()
    {
        return tile;
    }

    public bool isActive()
    {
        return gameObject.activeSelf;
    }

    public void Clean()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameEventManager.instance.clearUnits -= DestroyIfRemoved;
        GameEventManager.instance.removeKillUnits -= KillUnit;
        GameEventManager.instance.newGame -= Clean;
    }
}
