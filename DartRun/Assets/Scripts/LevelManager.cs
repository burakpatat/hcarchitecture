using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Transform[] _levels;

    [SerializeField] int _currentLevel = 0;
    int _maxLevel;

    public Transform[] Levels { get { return _levels; } }
    public int CurrentLevel { get { return _currentLevel; } }
    public int MaxLevel { get { return _maxLevel; } }

    public event Action EventNextLevel;
    void Start() 
    {
        _levels = GetTopLevelChildren(transform);
        _maxLevel = _levels.Length;
    }

    public void NextLevel() 
    {
        _levels[_currentLevel % _maxLevel].gameObject.SetActive(false);
        _levels[++_currentLevel % _maxLevel].gameObject.SetActive(true);

        EventNextLevel?.Invoke();
    }

    public Transform[] GetTopLevelChildren(Transform Parent)
    {
        Transform[] Children = new Transform[Parent.childCount];
        for (int i = 0; i < Parent.childCount; i++)
        {
            Children[i] = Parent.GetChild(i);
        }
        return Children;
    }
    public Transform GetCurrentLevel()
    {
        return _levels[_currentLevel % _maxLevel];
    }
}
