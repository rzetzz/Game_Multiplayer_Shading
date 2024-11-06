using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGane : MonoBehaviour
{
    
    [SerializeField] TMP_InputField inputIp;
    [SerializeField] TextMeshProUGUI text;
    public string selectedStage = "Stage1";
    
    private void Start() 
    {
        // NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnectedCallback; 
    }
    private void Update() 
    {   
        text.text = GetLocalIPAddress();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
        GetLocalIPAddress(),  
        (ushort)7770,
        "0.0.0.0" 
        );

        NetworkManager.Singleton.StartHost();
        
    }

    public void StartClient()
    {

        Debug.Log(inputIp.text);
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = inputIp.text;
        
        NetworkManager.Singleton.StartClient();
    }
    
    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            // Cek apakah IP-nya adalah IPv4 dan bukan loopback
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "No network adapters with an IPv4 address in the system!";
    }

    public void SwitchScene()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(selectedStage, LoadSceneMode.Single);
        }
    }

    private void HandleClientConnectedCallback(ulong clientId)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(selectedStage, LoadSceneMode.Single); 
        }
        
    }
    
}
