using System.Collections;
using UnityEngine;

public class FruitSpawner : Spawner
{
    public override void OnAddDifficulty()
    {
        ReduceMaxSpawnTime();
    }
}
