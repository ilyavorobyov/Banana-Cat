using UnityEngine;

public class SpeedBoosterSpawner : Spawner
{
    public override void OnAddDifficulty()
    {
        AddDropChance();
    }
}