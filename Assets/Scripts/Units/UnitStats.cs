using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SyncVar] protected int _curHealth;
    public Stat Damage;
    public Stat Armor;
    public Stat MoveSpeed;


    public virtual int CurHealth
    {
        get 
        { 
            return _curHealth; 
        }
        protected set 
        { 
            _curHealth = value; 
        }
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= Armor.GetValue();
        if (damage > 0)
        {
            CurHealth -= damage;
            if (CurHealth <= 0)
            {
                CurHealth = 0;
            }
        }
    }
    public void AddHealth(int amount)
    {
        CurHealth += amount;
        if (CurHealth > _maxHealth)
        {
            CurHealth = _maxHealth;
        }
    }

    public void SetHealthRate(float rate)
    {
        CurHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
}
