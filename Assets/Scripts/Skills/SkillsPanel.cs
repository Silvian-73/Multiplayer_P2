using UnityEngine;
using UnityEngine.Networking;

public class SkillsPanel : MonoBehaviour
{

    #region Singleton
    public static SkillsPanel Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of SkillsPanel found!");
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] private SkillPanelItem[] _items;

    private UnitSkills _skills;
    private void Start()
    {
        MyNetworkManager manager = NetworkManager.singleton as MyNetworkManager;
        gameObject.SetActive(!manager.ServerMode);
    }
    public void SetSkills(UnitSkills skills)
    {
        _skills = skills;
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetSkill(i < skills.Count ? skills[i] : null);
        }
    }

    private void Update()
    {
        if (_skills != null)
        {
            bool inCast = _skills.InCast;
            for (int i = 0; i < _skills.Count && i < _items.Length; i++)
            {
                _items[i].SetCastTime(_skills[i].CastDelay);
                _items[i].SetHolder(inCast || _skills[i].CastDelay > 0 || _skills[i].CooldownDelay > 0);
            }
        }
    }
}
