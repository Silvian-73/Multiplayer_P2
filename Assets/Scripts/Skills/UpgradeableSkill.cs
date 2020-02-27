using System;
using UnityEngine.Networking;

public class UpgradeableSkill : Skill
{
    public event Action<UpgradeableSkill, int> OnSetLevel;

    [SyncVar(hook = "LevelHook")] int _level = 1;
    public virtual int Level
    {
        get 
        { 
            return _level; 
        }
        set
        {
            _level = value;
            if (OnSetLevel != null)
            {
                OnSetLevel.Invoke(this, Level);
            }
        }
    }

    void LevelHook(int newLevel)
    {
        Level = newLevel;
    }
}
