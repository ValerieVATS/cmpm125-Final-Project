using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Paused, LevelComplete, GameOver }

/// <summary>
/// Owns overall game state (playing / paused / won / game over) and the stats
/// that carry between level scenes. Persists across scene loads.
/// Put a GameManager prefab in EVERY scene so any level can be tested on its own;
/// extra copies destroy themselves automatically.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>UI scripts (pause panel, game-over screen, HUD) subscribe to this.</summary>
    public static event Action<GameState> OnStateChanged;

    [Header("Scene names (must be in Build Profiles > Scene List)")]
    public string mainMenuScene = "MainMenu";
    public string creditsScene = "Credits";

    public GameState State { get; private set; } = GameState.Playing;

    // ---- Stats carried between scenes ----
    public int TotalStars { get; private set; }
    public int Deaths { get; private set; }
    public float PlayTime { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            Instance = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State == GameState.Playing) Pause();
            else if (State == GameState.Paused) Resume();
        }

        if (State == GameState.Playing)
        {
            PlayTime += Time.deltaTime;
        }
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetState(GameState.Playing);
    }

    void SetState(GameState newState)
    {
        State = newState;
        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;
        OnStateChanged?.Invoke(newState);
    }

    // ---- Called by gameplay scripts ----

    public void AddStars(int amount) => TotalStars += amount;

    public void RegisterDeath() => Deaths++;

    /// <summary>Call when the player reaches the exit with enough stars / solves the puzzle.</summary>
    public void CompleteLevel() => SetState(GameState.LevelComplete);

    /// <summary>Call when health hits zero (Valerie's health system).</summary>
    public void GameOver() => SetState(GameState.GameOver);

    // ---- Called by UI buttons ----

    public void Pause() => SetState(GameState.Paused);

    public void Resume() => SetState(GameState.Playing);

    public void LoadNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(creditsScene); // finished the final level
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Start a fresh run from the menu: clears carried stats.</summary>
    public void StartNewRun(string firstLevelScene)
    {
        TotalStars = 0;
        Deaths = 0;
        PlayTime = 0f;
        SceneManager.LoadScene(firstLevelScene);
    }

    public void QuitToMenu() => SceneManager.LoadScene(mainMenuScene);
}
