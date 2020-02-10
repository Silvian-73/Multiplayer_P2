using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SyncVar] protected int _curHealth;
    public Stat Damage;
    public Stat Armor;
    public Stat MoveSpeed;
    public override void OnStartServer()
    {
        _curHealth = _maxHealth;
    }

    public int CurHealth 
    { 
        get 
        { 
            return _curHealth;
        } 
    }
    public void SetHealthRate(float rate)
    {
        _curHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
    public virtual void TakeDamage(int damage)
    {
        damage -= Armor.GetValue();
        if (damage > 0)
        {
            _curHealth -= damage;
            if (_curHealth <= 0)
            {
                _curHealth = 0;
            }
        } 
    }
}
