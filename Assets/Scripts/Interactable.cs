using UnityEngine;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour
{
    public Transform InteractionTransform;
    [SerializeField] float _radius = 2f;
    private bool _hasInteract = true;
    public bool HasInteract
    {
        get 
        { 
            return _hasInteract; 
        }
        set 
        { 
            _hasInteract = value; 
        }
    }

    public virtual bool Interact(GameObject user)
    {
        return false;
    }
    public virtual float GetInteractDistance(GameObject user)
    {
        return _radius;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(InteractionTransform.position, _radius);
    }
}
