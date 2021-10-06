using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState { Idle, Run, Win, Lose }
public class Player : Singleton<Player>, IAnimationScripts, IEventScripts
{
    public PlayerState _playerState;

    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();

        GameManager.Instance.EventMenu += OnMenu;
        GameManager.Instance.EventPlay += OnPlay;
        GameManager.Instance.EventFinish += OnFinish;
    }
    public void OnMenu()
    {
        SetTriggerAnimation(PlayerState.Idle);
        transform.position = Vector3.zero;
    }

    public void OnPlay()
    {
        SetTriggerAnimation(PlayerState.Run);
    }

    public void OnFinish()
    {
        SetTriggerAnimation(PlayerState.Win);
    }

    public void OnLose()
    {

    }

    public void SetTriggerAnimation(Enum _enum)
    {
        //situation
        _playerState = (PlayerState)_enum;

        _animator.SetTrigger(Enum.GetName(typeof(PlayerState), _enum));
    }

    public void SetBoolAnimation(Enum _enum, bool _state)
    {
        //situation
        _playerState = (PlayerState)_enum;

        _animator.SetBool(Enum.GetName(typeof(PlayerState), _enum), _state);
    }
}
