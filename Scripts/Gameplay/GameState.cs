using DG.Tweening;
using System;
using UnityEngine;

public class GameState : MonoBehaviour, IInitializable
{
    public static GameState Instance;

    public Action GameStarted;
    public Action GamePaused;
    public Action GameUnpaused;
    public Action GameFinished;

    public Action ScoreAdded;

    public enum State
    { 
        InGame,
        Paused,
        Finished
    }
    public State CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void Initialize()
    {
        StartGame();
    }
    public void StartGame()
    {
        GameStarted?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
        PlayerInput.Instance.IsEnabled = true;
    }
    public void PauseGame()
    {
        GamePaused?.Invoke();
        CurrentState = State.Paused;
        Time.timeScale = 0.0f;
        PlayerInput.Instance.IsEnabled = false;
    }
    public void UnpauseGame()
    {
        GameUnpaused?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
        PlayerInput.Instance.IsEnabled = true;
    }
    public void FinishGame()
    {
        GameFinished?.Invoke();
        CurrentState = State.Finished;
        //Time.timeScale = 0.0f;
        PlayerInput.Instance.IsEnabled = false;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);

        /*if (AudioVibrationManager.Instance.IsVibrationEnabled)
            Handheld.Vibrate();*/
    }
}