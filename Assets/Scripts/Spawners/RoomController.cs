using UnityEngine;

public class RoomController : MonoBehaviour
{
    public RandomSpawner[] spawners;
    public UpgradeSpawner upgradeSpawner;
    public GameObject enemy;

    private int aliveEnemies;
    private int levelCount;
    private bool upgradesActive;

	public void Awake()
    {
        upgradesActive = false;
        aliveEnemies = 0;
        levelCount = 0;
        upgradeSpawner.DeactivateUpgrades();
	}
	
	public void FixedUpdate()
    {
        if (aliveEnemies == 0 && !upgradesActive)
        {
            upgradesActive = true;
            upgradeSpawner.ActivateUpgrades();
        }
        else if(upgradesActive)
        {
            if(upgradeSpawner.Collected())
            {
                upgradesActive = false;
                StartNewRound();
            }
        }
	}

    public void StartNewRound()
    {
        levelCount++;
        aliveEnemies = GetEnemyCount();
        int enemiesPerSpawner = aliveEnemies/ spawners.Length;
        int enemiesSpawned = 0;

        for(int i = 0; i < spawners.Length; i++)
        {
            int spawnCount = enemiesPerSpawner;
            if (i == spawners.Length-1)
                spawnCount = aliveEnemies - enemiesSpawned;

            spawners[i].Spawn(enemy, this, spawnCount);
            enemiesSpawned += spawnCount;
        }
    }

    private int GetEnemyCount()
    {
        //return (levelCount + spawners.Length)* 2;
        return 1;
    }

    public void EnemySpawned(int count)
    {
        if (count <= 0)
            return;
        aliveEnemies += count;
    }

    public void EnemyKilled(int count)
    {
        if (count <= 0)
            return;
        aliveEnemies -= count;
    }
}