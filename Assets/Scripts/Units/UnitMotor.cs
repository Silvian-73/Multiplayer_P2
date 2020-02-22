using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour 
{

    private NavMeshAgent _agent;
    private Transform _target;

    private void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }
    void Start () 
    {
        
	}
	public void MoveToPoint(Vector3 point) 
    {
        _agent.SetDestination(point);
    }

    public void FollowTarget(Interactable newTarget, float interactDistance)
    {
        _agent.stoppingDistance = interactDistance;
        _target = newTarget.InteractionTransform;
    }
    public void StopFollowingTarget()
    {
        _agent.stoppingDistance = 0f;
        _agent.ResetPath();
        _target = null;
    }
    private void Update()
    {
        if (_target != null)
        {
            if (_agent.velocity.magnitude == 0)
            {
                FaceTarget();
            }
            _agent.SetDestination(_target.position);
        }
    }
    private void FaceTarget()
    {
        Vector3 direction = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    public void SetMoveSpeed (int speed)
    {
        _agent.speed = speed;
    }
}
