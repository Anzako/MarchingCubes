using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    private GameManager gameManager;
    private PlayerSpawner pSpawner;
    private AsteroidsSpawner aSpawner;

    [SerializeField] private Button restartButton;
    private int actualRound = 0;
    public int amount;

    public int[] asteroidsInRound;

    private void Start()
    {
        GameManager.OnStateChanged += GameManagerOnStateChanged;
        restartButton.onClick.AddListener(RestartGame);
        pSpawner = PlayerSpawner.Instance;
        aSpawner = AsteroidsSpawner.Instance;
        gameManager = GameManager.Instance;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= GameManagerOnStateChanged;
    }

    public void GameManagerOnStateChanged(GameState state)
    {
        restartButton.gameObject.SetActive(state == GameState.GameOver);
    }

    public void StartGame()
    {
        actualRound = 0;
        pSpawner.SpawnPlayer(); 
        StartRound(actualRound);
    }

    private void StartRound(int round)
    {
        aSpawner.SpawnAsteroids(asteroidsInRound[round]);
    }

    public void GameOver()
    {
        ScoreManager.instance.ResetScore();
        gameManager.ChangeState(GameState.GameOver);
    }
    public void RestartGame()
    {
        aSpawner.ReturnAsteroidsToPool();
        gameManager.ChangeState(GameState.Game);
    }

    public void EndRound()
    {
        actualRound++;
        if (actualRound >= asteroidsInRound.Length)
        {
            EndGame();
            return;
        } 

        // Do as IEnumerator to wait for next round
        StartRound(actualRound);
    }

    private void EndGame()
    {
        Debug.Log("Game ends");
    }
}
