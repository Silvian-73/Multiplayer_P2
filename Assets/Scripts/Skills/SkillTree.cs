using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    #region Singleton
    public static SkillTree Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of SkillTree found!");
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] private SkillTreeItem[] _items;
    [SerializeField] private Text _skillPointsText;

    private StatsManager _manager;
    private int _curSkillPoints;

    private void CheckManagerChanges()
    {
        if (_curSkillPoints != _manager.SkillPoints)
        {
            _curSkillPoints = _manager.SkillPoints;
            _skillPointsText.text = _curSkillPoints.ToString();
            SetUpgradableSkills(_curSkillPoints > 0);
        }
    }
    void Update()
    {
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
    void SetUpgradableSkills(bool active)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetUpgradable(active);
        }
    }
    public void SetCharacter(Character character)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetSkill(i < character.UnitSkills.Count ? character.UnitSkills[i] as UpgradeableSkill : null);
        }
        if (_manager != null)
        {
            CheckManagerChanges();
        }
    }
    public void UpgradeSkill(SkillTreeItem skillItem)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == skillItem)
            {
                _manager.CmdUpgradeSkill(i);
                break;
            }
        }
    }
}
