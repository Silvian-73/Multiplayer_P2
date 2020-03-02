using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private int _level = 1;
    private int _statPoints;
    private int _skillPoints;
    private float _exp;
    private float _nextLevelExp = 100;

    private UserData _data;

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
            _manager.SkillPoints = _skillPoints;

        }
    }
    public void Load(UserData data)
    {
        _data = data;
        if (_data.Level > 0)
        {
            _level = _data.Level;
        }

        _statPoints = _data.StatPoints;
        _skillPoints = _data.SkillPoints;
        _exp = _data.Exp;
        if (_data.NextLevelExp > 0)
        {
            _nextLevelExp = _data.NextLevelExp;
        }
    }
    public void AddExp(float addExp)
    {
        _data.Exp = _exp += addExp;
        while (_exp >= _nextLevelExp)
        {
            _data.Exp = _exp -= _nextLevelExp;
            LevelUP();
        }
        if (_manager != null)
        {
            _manager.Exp = _exp;
            _manager.Level = _level;
            _manager.NextLevelExp = _nextLevelExp;
            _manager.StatPoints = _statPoints;
            _manager.SkillPoints = _skillPoints;
        }
    }
    private void LevelUP()
    {
        _data.Level = ++_level;
        _data.NextLevelExp = _nextLevelExp += 100f;
        _data.StatPoints = _statPoints += 3;
        _data.SkillPoints = _skillPoints += 1;
    }
    public bool RemoveStatPoint()
    {
        if (_statPoints > 0)
        {
            _data.StatPoints = --_statPoints;
            if (_manager != null) _manager.StatPoints = _statPoints;
            return true;
        }
        return false;
    }
    public bool RemoveSkillPoint()
    {
        if (_skillPoints > 0)
        {
            _data.SkillPoints = --_skillPoints;
            if (_manager != null)
            {
                _manager.SkillPoints = _skillPoints;
            }

            return true;
        }
        return false;
    }
}
