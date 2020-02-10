using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    #region Singleton
    public static StatsUI Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of StatsUI found!");
            return;
        }
        Instance = this;
    }
    #endregion
    [SerializeField] private GameObject _statsUI;
    [SerializeField] private StatItem _damageStat;
    [SerializeField] private StatItem _armorStat;
    [SerializeField] private StatItem _moveSpeedStat;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _statPointsText;

    private StatsManager _manager;
    private int _curDamage;
    private int _curArmor;
    private int _curMoveSpeed;
    private int _curLevel;
    private int _curStatPoints;
    private float _curExp;
    private float _nextLevelExp;

    void Start()
    {
        _statsUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetButtonDown("Stats"))
        {
            _statsUI.SetActive(!_statsUI.activeSelf);
        }
        if (_manager != null)
        {
            CheckManagerChanges();
        }
    }
    public void SetManager(StatsManager statsManager)
    {
        _manager = statsManager;
        CheckManagerChanges();
    }
    private void CheckManagerChanges()
    {
        // stat changes
        if (_curDamage != _manager.Damage)
        {
            _curDamage = _manager.Damage;
            _damageStat.ChangeStat(_curDamage);
        }
        if (_curArmor != _manager.Armor)
        {
            _curArmor = _manager.Armor;
            _armorStat.ChangeStat(_curArmor);
        }
        if (_curMoveSpeed != _manager.MoveSpeed)
        {
            _curMoveSpeed = _manager.MoveSpeed;
            _moveSpeedStat.ChangeStat(_curMoveSpeed);
        }
        if (_curLevel != _manager.Level)
        {
            _curLevel = _manager.Level;
            _levelText.text = _curLevel.ToString();
        }
        if (_curExp != _manager.Exp)
        {
            _curExp = _manager.Exp;
        }
        if (_nextLevelExp != _manager.NextLevelExp)
        {
            _nextLevelExp = _manager.NextLevelExp;
        }
        if (_curStatPoints != _manager.StatPoints)
        {
            _curStatPoints = _manager.StatPoints;
            _statPointsText.text = _curStatPoints.ToString();
            if (_curStatPoints > 0)
            {
                SetUpgradableStats(true);
            }
            else
            {
                SetUpgradableStats(false);
            }
        }
    }
    private void SetUpgradableStats(bool active)
    {
        _damageStat.SetUpgradable(active);
        _armorStat.SetUpgradable(active);
        _moveSpeedStat.SetUpgradable(active);
    }
    public void UpgradeStat(StatItem stat)
    {
        if (stat == _damageStat)
        {
            _manager.CmdUpgradeStat((int)StatType.Damage);
        }
        else if (stat == _armorStat)
        {
            _manager.CmdUpgradeStat((int)StatType.Armor);
        }
        else if (stat == _moveSpeedStat)
        {
            _manager.CmdUpgradeStat((int)StatType.MoveSpeed);
        }
    }
}
