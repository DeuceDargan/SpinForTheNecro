using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObject/Wave")]

public class WaveOfEnemies : ScriptableObject
{
    public List<UnitSObject> wave = new List<UnitSObject>();

    private void OnEnable()
    {
        // for (int x = 0; x < wave.Count; x++)
        // {
        //     wave[x] = Instantiate(wave[x]);
        // }
    }

    public void increaseHealth (int h)
    {
        foreach (var enemy in wave)
        {
            enemy.health += h;
        }
    }
}
