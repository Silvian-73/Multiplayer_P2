using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{

    [SyncVar] public int Damage;
    [SyncVar] public int Armor;
    [SyncVar] public int MoveSpeed;

    [SyncVar] public int Level;
    [SyncVar] public int StatPoints;
    [SyncVar] public int SkillPoints;
    [SyncVar] public float Exp;
    [SyncVar] public float NextLevelExp;

    public Player Player;

    [Command]
    public void CmdUpgradeStat(int stat)
    {
        if (Player.Progress.RemoveStatPoint())
        {
            switch (stat)
            {
                case (int)StatType.Damage: 
                    Player.Character.Stats.Damage.BaseValue++; 
                    break;
                case (int)StatType.Armor: 
                    Player.Character.Stats.Armor.BaseValue++; 
                    break;
                case (int)StatType.MoveSpeed: 
                    Player.Character.Stats.MoveSpeed.BaseValue++; 
                    break;
            }
        }
    }

    [Command]
    public void CmdUpgradeSkill(int index)
    {
        if (Player.Progress.RemoveSkillPoint())
        {
            UpgradeableSkill skill = Player.Character.UnitSkills[index] as UpgradeableSkill;
            if (skill != null)
            {
                skill.Level++;
            }
        }
    }
}
