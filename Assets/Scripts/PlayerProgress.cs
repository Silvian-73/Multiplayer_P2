using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private int _level = 1;
    private int _statPoints;
    private float _exp;
    private float _nextLevelExp = 100;

    private StatsManager _manager;
    public StatsManager Manager
    {
        set
        {
            _manager = value;
            _manager.Exp = _exp;
            _manager.NextLevelExp = _nextLevelExp;
            _manager.Level = _level;
            _manager.StatPoints = _statPoints;
        }
    }

    public void AddExp(float addExp)
    {
        _exp += addExp;
        while (_exp >= _nextLevelExp)
        {
            _exp -= _nextLevelExp;
            LevelUP();
        }
        if (_manager != null)
        {
            _manager.Exp = _exp;
            _manager.Level = _level;
            _manager.NextLevelExp = _nextLevelExp;
            _manager.StatPoints = _statPoints;
        }
    }
    private void LevelUP()
    {
        _level++;
        _nextLevelExp += 100f;
        _statPoints += 3;
    }
    public bool RemoveStatPoint()
    {
        if (_statPoints > 0)
        {
            _statPoints--;
            if (_manager != null) _manager.StatPoints = _statPoints;
            return true;
        }
        return false;
    }
}
