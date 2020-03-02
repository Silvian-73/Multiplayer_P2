using System;
using UnityEngine;

[Serializable]
public class UnitSkills 
{
    [SerializeField] private Skill[] _skills;

    private UserData _data;

    public Skill this[int index]
    {
        get 
        { 
            return _skills[index]; 
        }
        set 
        { 
            _skills[index] = value; 
        }
    }
    public int Count 
    { 
        get 
        { 
            return _skills.Length; 
        } 
    }

    public bool InCast
    {
        get
        {
            foreach (Skill skill in _skills)
            {
                if (skill.CastDelay > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public void Load(UserData data)
    {
        _data = data;
        for (int i = 0; i < _skills.Length; i++)
        {
            UpgradeableSkill skill = _skills[i] as UpgradeableSkill;
            if (i >= data.skills.Count)
            {
                data.skills.Add(skill.Level);
            }
            else
            {
                skill.Level = data.skills[i];
            }

            skill.OnSetLevel += ChangeLevel;
        }
    }

    void ChangeLevel(UpgradeableSkill skill, int newLevel)
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            if (_skills[i] == skill)
            {
                _data.skills[i] = newLevel;
                break;
            }
        }
    }
}
