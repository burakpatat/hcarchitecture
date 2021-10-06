using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum GameState {Menu,Play,Finish,Lose }

public class GameManager : Singleton<GameManager>
{
    GameState _gameState;
    public GameState GameState{ get { return _gameState; } }

    public event Action EventMenu;
    public event Action EventPlay;
    public event Action EventFinish;
    public event Action EventLose;

    void Start()
    {
        LevelManager.Instance.EventNextLevel += OnNextLevel;
        SetMenu();
    }
    public void SetMenu()
    {
        _gameState = GameState.Menu;
        EventMenu?.Invoke();
    }

    public void SetPlay()
    {
        _gameState = GameState.Play;
        EventPlay?.Invoke();
    }
    public void SetFinish()
    {
        _gameState = GameState.Finish;
        EventFinish?.Invoke();
    }

    public void SetLose()
    {
        _gameState = GameState.Lose;
        EventLose?.Invoke();
    }

    public void OnNextLevel() 
    {
        SetMenu();
    }

}
