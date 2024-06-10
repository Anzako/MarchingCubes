using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private GameManager gameManager;
    private PlayerSpawner playerSpawner;
    private AsteroidsSpawner asteroidsSpawner;
    private EnemySpawner enemySpawner;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIController HUDController;

    private bool gameStarted = false;
    private int actualRound = 0;
    private float elapsedTime = 0;
    private float elapsedRoundTime = 0;
    public float timeToSpawnEnemy;

    private void Start()
    {
        playerSpawner = PlayerSpawner.Instance;
        asteroidsSpawner = AsteroidsSpawner.Instance;
        gameManager = GameManager.Instance;
        enemySpawner = EnemySpawner.Instance;
    }

    private void Update()
    {
        if (!gameStarted) return;

        elapsedTime += Time.deltaTime;
        elapsedRoundTime += Time.deltaTime;

        if (elapsedRoundTime >= timeToSpawnEnemy)
        {
            SpawnEnemy();
            elapsedRoundTime = 0;
        }
    }

    #region Game Logic
    public void StartGame()
    {
        CleanScene();
        MetaBalls.Instance.ResetMetaballsParameters();
        gameStarted = true;
        actualRound = 0;
        elapsedTime = 0;

        playerSpawner.SpawnPlayer(); 
        StartRound(actualRound);
    }

    private void StartRound(int round)
    {
        elapsedRoundTime = 0;
        asteroidsSpawner.SpawnAsteroids((actualRound + 1) * 2);
        HUDController.SetWave(round + 1);
    }

    public void RestartGame()
    {
        gameManager.ChangeState(GameState.StartGame);
    }

    public void CleanScene()
    {
        asteroidsSpawner.DestroyAllAsteroids();
        enemySpawner.DestroyAllEnemies();
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.playerProjectile);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.rocket);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.laser);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.healUpItem);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.rocketItem);
        ScoreManager.Instance.ResetScore();
    }

    public void EndRound()
    {
        actualRound++;

        // Do as IEnumerator to wait for next round
        StartRound(actualRound);
    }

    public void EndGame()
    {
        gameManager.ChangeState(GameState.EndGame);
        gameStarted = false;
    }
    #endregion

    private void SpawnEnemy()
    {
        enemySpawner.SpawnEnemy();
    }

    public Vector3 GetPlayerPosition()
    {
        return playerController.transform.position;
    }
}
