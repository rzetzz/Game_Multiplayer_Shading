using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class RelayControl : MonoBehaviour
{
    public static RelayControl instance;
    LoadingControl loading;
    public string joinCode;
    public string playerName;
    public string selectedStage = "Stage1";
    [SerializeField] TextMeshProUGUI code;
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }    
    }
    private void Start() 
    {
        loading = GetComponentInChildren<LoadingControl>();
        loading.DisableLoading();
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnectedCallback;    
    }

    public async void LogIn(string name)
    {
        this.playerName = name;
        InitializationOptions init = new InitializationOptions();
        init.SetProfile(name);

        await UnityServices.InitializeAsync(init); 
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed In " + AuthenticationService.Instance.PlayerId + " " + this.playerName);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay(int maxPlayer)
    {
        try {

            loading.StartLoading();
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayer);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(selectedStage , LoadSceneMode.Single);

            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnHostSceneLoaded;
            // loading.EndLoading();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    private void OnHostSceneLoaded(ulong clientId,string sceneName, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        
        loading.EndLoading();
        code.gameObject.SetActive(true);
        code.text = "Join Code : " + joinCode;
        
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnHostSceneLoaded;
    }

    

    public async void JoinRelay(string code)
    {
        try {
            loading.StartLoading();
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
            // loading.EndLoading();
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnClientSceneLoaded;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    private void OnClientSceneLoaded(ulong clientId,string sceneName, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        
        loading.EndLoading();

        
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnClientSceneLoaded;
    }

    private void HandleClientConnectedCallback(ulong clientId)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(selectedStage, LoadSceneMode.Single); 
        }
        
    }
}
