using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    #region Singleton
    public static EquipmentUI Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of EquipmentUI found!");
            return;
        }
        Instance = this;
    }
    #endregion
    [SerializeField] GameObject _equipmentUI;
    [Space]
    [SerializeField] EquipmentSlot _headSlot;
    [SerializeField] EquipmentSlot _chestSlot;
    [SerializeField] EquipmentSlot _legsSlot;
    [SerializeField] EquipmentSlot _rightHandSlot;
    [SerializeField] EquipmentSlot _leftHandSlot;

    private Equipment _equipment;
    private EquipmentSlot[] _slots;

    private void Start()
    {
        _equipmentUI.SetActive(false);
        _slots = new EquipmentSlot[System.Enum.GetValues(typeof(EquipmentSlotType)).Length];
        _slots[(int)EquipmentSlotType.Chest] = _chestSlot;
        _slots[(int)EquipmentSlotType.Head] = _headSlot;
        _slots[(int)EquipmentSlotType.LeftHand] = _leftHandSlot;
        _slots[(int)EquipmentSlotType.Legs] = _legsSlot;
        _slots[(int)EquipmentSlotType.RightHand] = _rightHandSlot;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Equipment"))
        {
            _equipmentUI.SetActive(!_equipmentUI.activeSelf);
        }
    }

    public void SetEquipment(Equipment newEquipment)
    {
        _equipment = newEquipment;
        _equipment.onItemChanged += ItemChanged;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
            { 
                _slots[i].equipment = _equipment; 
            }
        }
        ItemChanged(0, 0);
    }

    private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex)
    {
        for (int i = 0; i < _slots.Length; i++) _slots[i].ClearSlot();
        for (int i = 0; i < _equipment.Items.Count; i++)
        {
            _slots[(int)((EquipmentItem)_equipment.Items[i]).equipSlot].SetItem(_equipment.Items[i]);
        }
    }
}
