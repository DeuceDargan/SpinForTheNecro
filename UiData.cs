using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUiData", menuName = "ScriptableObject/UiData")]

public class UiData : ScriptableObject
{
    public int wave;
    public int startSpins;
    public int spinsLeft;
    public int gold;
    public int income;
    public int incomeTier;
    public int maxIncomeTier;
    public int incomeInterval;
    public float healthScaler;
    public float dmgScaler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateIncome()
    {
        gold += (income + incomeTier);
    }

    public void UpdateIncomeTier()
    {
        incomeTier = gold / incomeInterval;
        
        if (incomeTier > maxIncomeTier)
        {
            incomeTier = maxIncomeTier;
        }
    }
}
