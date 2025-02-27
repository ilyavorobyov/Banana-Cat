namespace Spawners
{
    public class SpeedBoosterSpawner : Spawner
    {
        public override void OnAddDifficulty()
        {
            AddDropChance();
        }
    }
}