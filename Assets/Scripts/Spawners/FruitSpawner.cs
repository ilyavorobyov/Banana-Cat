namespace Spawners
{
    public class FruitSpawner : Spawner
    {
        public override void OnAddDifficulty()
        {
            ReduceMaxSpawnTime();
        }
    }
}