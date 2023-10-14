using UnityEngine;

public class BuffHelmetSpawner : Spawner
{
    public override void OnAddDifficulty()
    {
        AddDropChance();
    }
}