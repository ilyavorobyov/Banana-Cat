namespace Spawners
{
    public class DangerousItemsSpawner : Spawner
    {
        public override void OnAddDifficulty()
        {
            AddDropChance();
        }
    }
}