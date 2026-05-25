using UnityEngine;
using TMPro;

public enum GameState { Preparation, Battle, RoundEnd, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }

    [SerializeField] private int totalRounds = 10;
    [SerializeField] private TextMeshProUGUI roundText;
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
        // show a "Start Wave" button in your UI
    }

    public void StartBattle()
    {
        State = GameState.Battle;
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
}