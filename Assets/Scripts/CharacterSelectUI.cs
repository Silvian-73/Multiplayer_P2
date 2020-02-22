using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelectUI : MonoBehaviour
{
    #region Singleton
    public static CharacterSelectUI Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of CharacterSelectUI found!");
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private GameObject _selectPanel;

    public void OpenPanel()
    {
        _loginPanel.SetActive(false);
        _selectPanel.SetActive(true);
    }

    public void SelectCharacter(NetworkIdentity characterIdentity)
    {
        CharacterSelect.Instance.SelectCharacter(characterIdentity.assetId);
    }
}
