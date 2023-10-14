using UnityEngine;

public class DangerousItemsSpawner : Spawner
{
    public override void OnAddDifficulty()
    {
        AddDropChance();
    }
}