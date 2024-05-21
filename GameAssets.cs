using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets i;

    private void Awake()
    {
        i = this;
    }
    
    [Header("Managers")]
    public SquareGrid board;
    public UnitManager unitManager;
    public UiManager uiManager;
    public ItemManager itemManager;

    [Header("Assets")]
    public GameObject unitPiece;
    public GameObject bossPiece;
    public GameObject draftCard;
}
