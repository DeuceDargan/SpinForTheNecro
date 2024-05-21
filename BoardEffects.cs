using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "ScriptableObject/BoardEffect")]
public class BoardEffects : ScriptableObject
{
    public enum Effect
    {
        WARCRY
    };

    public enum Targeting
    {
        ALLY, ALLIES, ENEMY, ENEMIES
    };

    public Sprite sprite;
    public string description;
    public Effect effect;
    public Targeting targeting;
    public int charges;
    public int cost;
    public GameObject display;
}
