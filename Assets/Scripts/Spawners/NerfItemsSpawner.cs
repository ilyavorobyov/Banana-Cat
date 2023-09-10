using UnityEngine;

public class NerfItemsSpawner : Spawner
{
    public override void AddDifficulty()
    {
        AddDropChance();
    }
}