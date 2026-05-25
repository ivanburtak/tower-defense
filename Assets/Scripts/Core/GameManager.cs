using UnityEngine;
using System;

namespace TowerDefense.Core
{
    /// <summary>
    /// Central game manager handling overall game state, health, money, and win/lose conditions.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int startingHealth = 20;
        [SerializeField] private int startingMoney = 500;
        [SerializeField] private int healthPerWave = 0; // Health restored per wave (0 = none)

        private int currentHealth;
        private int currentMoney;
        private bool gameOver = false;
        private bool gameWon = false;

        // Events
        public event Action<int> OnHealthChanged;
        public event Action<int> OnMoneyChanged;
        public event Action OnGameOver;
        public event Action OnGameWon;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            currentHealth = startingHealth;
            currentMoney = startingMoney;
            gameOver = false;
            gameWon = false;

            OnHealthChanged?.Invoke(currentHealth);
            OnMoneyChanged?.Invoke(currentMoney);
        }

        /// <summary>
        /// Add or subtract money from the player.
        /// </summary>
        public bool TrySpendMoney(int amount)
        {
            if (currentMoney >= amount)
            {
                currentMoney -= amount;
                OnMoneyChanged?.Invoke(currentMoney);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add money to the player (e.g., from defeating enemies).
        /// </summary>
        public void AddMoney(int amount)
        {
            currentMoney += amount;
            OnMoneyChanged?.Invoke(currentMoney);
        }

        /// <summary>
        /// Reduce player health and check for game over.
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (gameOver) return;

            currentHealth -= damage;
            OnHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                EndGame(false);
            }
        }

        /// <summary>
        /// Heal the player (used for wave progression or special events).
        /// </summary>
        public void Heal(int amount)
        {
            currentHealth = Mathf.Max(currentHealth + amount, startingHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        /// <summary>
        /// Called at the start of each wave to restore health if configured.
        /// </summary>
        public void OnWaveStart()
        {
            if (healthPerWave > 0)
            {
                Heal(healthPerWave);
            }
        }

        /// <summary>
        /// End the game with a win or lose condition.
        /// </summary>
        public void EndGame(bool playerWon)
        {
            if (gameOver || gameWon) return;

            if (playerWon)
            {
                gameWon = true;
                OnGameWon?.Invoke();
                Debug.Log("🎉 You Won!");
            }
            else
            {
                gameOver = true;
                OnGameOver?.Invoke();
                Debug.Log("💀 Game Over!");
            }

            Time.timeScale = 0f; // Pause the game
        }

        /// <summary>
        /// Reset the game for a new playthrough.
        /// </summary>
        public void ResetGame()
        {
            Time.timeScale = 1f;
            InitializeGame();
        }

        // Getters
        public int GetCurrentHealth() => currentHealth;
        public int GetCurrentMoney() => currentMoney;
        public bool IsGameOver() => gameOver;
        public bool IsGameWon() => gameWon;
        public int GetMaxHealth() => startingHealth;
    }
}
