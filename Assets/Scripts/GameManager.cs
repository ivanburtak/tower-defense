using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { None, Preparation, Battle, RoundEnd, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; } = GameState.None;

    [SerializeField] private int totalRounds = 10;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public int currentRound = 0;

    void Awake() => Instance = this;

    void Start() => EnterPreparation();

    public void EnterPreparation()
    {
        if (State == GameState.Preparation || State == GameState.GameOver)
        {
            Debug.LogError("Invalid game state when trying to enter preparation.");
            return;
        }

        currentRound++;
        State = GameState.Preparation;
        roundText.text = "Round: " + currentRound;
        startButton.interactable = true;
    }

    public void StartBattle()
    {
        if (State != GameState.Preparation)
        {
            Debug.LogError("Invalid game state when trying to start battle.");
            return;
        }
        State = GameState.Battle;
        startButton.interactable = false;
        WaveManager.Instance.StartWave(currentRound);
    }

    public void OnWaveComplete()
    {
        if (State != GameState.Battle)
        {
            Debug.LogError("Invalid game state when trying to complete wave.");
            return;
        }


        if (currentRound >= totalRounds)
        {
            State = GameState.GameOver;
            Time.timeScale = 0f;
            winScreen.SetActive(true);
            return;
        }

        State = GameState.RoundEnd;
        // small delay then next round
        Invoke(nameof(EnterPreparation), 2f);
    }

    public void OnBaseDead()
    {
        if (State != GameState.Battle)
        {
            Debug.LogError("Invalid game state when trying to end game.");
            return;
        }

        State = GameState.GameOver;
        Time.timeScale = 0f;
        loseScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}