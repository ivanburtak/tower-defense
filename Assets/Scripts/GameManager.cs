using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Preparation, Battle, RoundEnd, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }

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
        currentRound++;
        State = GameState.Preparation;
        roundText.text = "Round: " + currentRound;
        startButton.interactable = true;
    }

    public void StartBattle()
    {
        State = GameState.Battle;
        startButton.interactable = false;
        WaveManager.Instance.StartWave(currentRound);
    }

    public void OnWaveComplete()
    {
        State = GameState.RoundEnd;

        if (currentRound >= totalRounds)
        {
            winScreen.SetActive(true);
            return;
        }

        // small delay then next round
        Invoke(nameof(EnterPreparation), 2f);
    }

    public void OnBaseDead()
    {
        State = GameState.GameOver;
        loseScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}