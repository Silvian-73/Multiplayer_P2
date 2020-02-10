using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour {

    [SerializeField] protected Animator _animator;
    [SerializeField] protected NavMeshAgent _agent;
    private static readonly int Moving = Animator.StringToHash("Move");

    void FixedUpdate ()
    {
        if (_agent.velocity.magnitude == 0)
        {
            _animator.SetBool(Moving, false);
        }
        else
        {
            _animator.SetBool(Moving, true);
        }
    }

    //Placeholder functions for Animation events
    void Hit() 
    {
    }

    void FootR() 
    {
    }

    void FootL() 
    {
    }
}
