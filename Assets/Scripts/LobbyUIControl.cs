using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIControl : MonoBehaviour
{
    
    [SerializeField] TMP_InputField nameField,lobbyName,lobbyCode;
    [SerializeField] TextMeshProUGUI joinCode;
    
    [SerializeField] private Button enterButton, createButton, joinButton;
    [SerializeField] private Button stage1,stage2,stage3,stage4;
    
    private void Awake() 
    {
        enterButton.onClick.AddListener(() => {
            // GetComponent<LobbyControl>().LogIn(GetPlayerName());
            RelayControl.instance.LogIn(GetPlayerName());
            
        });
        createButton.onClick.AddListener(() =>
        {
            // GetComponent<LobbyControl>().CreateLobby(GetLobbyName(),10);
            // GetComponent<LobbyControl>().CreateRelay(10);
            RelayControl.instance.CreateRelay(10);
        });
        joinButton.onClick.AddListener(() =>
        {
            // GetComponent<LobbyControl>().JoinLobbyByCode(GetLobbyCode());
            // GetComponent<LobbyControl>().JoinRelay(GetLobbyCode());
            RelayControl.instance.JoinRelay(GetLobbyCode());
        });

        stage1.onClick.AddListener(() => {
            RelayControl.instance.selectedStage = "Stage1";
        });
        stage2.onClick.AddListener(() => {
            RelayControl.instance.selectedStage = "Stage2";
        });
        stage3.onClick.AddListener(() => {
            RelayControl.instance.selectedStage = "Stage3";
        });
        stage4.onClick.AddListener(() => {
            RelayControl.instance.selectedStage = "Stage4";
        });
    }
    public string GetPlayerName()
    {
        return nameField.text;
    }

    public string GetLobbyName()
    {
        return lobbyName.text;
    }

    public string GetLobbyCode()
    {
        return lobbyCode.text;
    }

    private void Update() 
    {
        
    }
}
