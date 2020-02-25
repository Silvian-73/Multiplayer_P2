using UnityEngine;
using UnityEngine.UI;

public class SkillPanelItem : MonoBehaviour
{

    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _holder;
    [SerializeField] private Text _timerText;

    public void SetSkill(Skill skill)
    {
        if (skill != null)
        {
            _icon.sprite = skill.icon;
            _holder.SetActive(false);
            _timerText.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetHolder(bool active)
    {
        _holder.SetActive(active);
    }

    public void SetCastTime(float time)
    {
        _timerText.text = ((int)time).ToString();
        _timerText.gameObject.SetActive(time > 0);
    }
}
