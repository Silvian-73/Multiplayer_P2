using UnityEngine;
using UnityEngine.Networking;

public class NetUnitSetup : NetworkBehaviour
{

    [SerializeField] private MonoBehaviour[] _disableBehaviours;

    void Awake()
    {
        for (int i = 0; i < _disableBehaviours.Length; i++)
        {
            _disableBehaviours[i].enabled = false;
        }
    }
    public override void OnStartServer()
    {
        for (int i = 0; i < _disableBehaviours.Length; i++)
        {
            _disableBehaviours[i].enabled = true;
        }
    }
}
