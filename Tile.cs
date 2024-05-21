using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer theRenderer;
    [SerializeField] private GameObject highlight;
    [SerializeField] private bool isHighlight;

    private UnitDisplay unit;
    private Vector2 pos;
    private bool removeMode;

    private void Awake()
    {
        GameEventManager.instance.clearUnits += ClearUnit;
        GameEventManager.instance.removeUnitModeOn += RemoveModeOn;
        GameEventManager.instance.removeUnitModeOff += RemoveModeOff;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (unit != null && isHighlight && Input.GetKeyDown(KeyCode.Mouse0) && !unit.GetUnit().isEnemy && removeMode)
        {
            UiManager ui = GameAssets.i.uiManager;

            unit.RemoveUnit();
            GameEventManager.instance.RemoveUnitModeOff();
            ui.GetPlayerUiData().gold -= ui.removeCost;
        }
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
        isHighlight = true;
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
        isHighlight = false;
    }

    public void Init(bool isOffset)
    {
        theRenderer.color = isOffset ? offsetColor : baseColor;
    }

    public void SetUnit(UnitDisplay u)
    {
        unit = u;
    }

    public void ClearUnit()
    {
        unit = null;
    }

    public UnitDisplay GetUnit()
    {
        return unit;
    }

    public void SetPos(Vector2 p)
    {
        pos = p;
    }

    public Vector2 GetPos()
    {
        return pos;
    }

    public void RemoveModeOn()
    {
        removeMode = true;
    }

    public void RemoveModeOff()
    {
        removeMode = false;
    }
}
