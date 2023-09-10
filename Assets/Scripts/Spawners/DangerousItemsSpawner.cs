using UnityEngine;

public class DangerousItemsSpawner : Spawner
{
    public override void AddDifficulty()
    {
        AddDropChance();
    }
}