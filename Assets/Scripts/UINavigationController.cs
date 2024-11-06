using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigationController : MonoBehaviour
{
    
    [SerializeField] GameObject authMenu, mainMenu, createMenu, joinMenu, chooseMenu;

    private Dictionary<UiPanel,GameObject> panels;
    public enum UiPanel
    {
        MainMenu,
        AuthMenu,
        ChooseMenu,
        CreateLobbyMenu,
        JoinLobbyMenu,
    }
    private void Start() {
        panels = new Dictionary<UiPanel, GameObject>
        {
            {UiPanel.AuthMenu , authMenu},
            {UiPanel.MainMenu , mainMenu},
            {UiPanel.CreateLobbyMenu , createMenu},
            {UiPanel.JoinLobbyMenu , joinMenu},
            {UiPanel.ChooseMenu , chooseMenu},
        };

        ShowPanel(UiPanel.MainMenu);
    }

    public void ShowPanel(UiPanel whatPanel)
    {
        foreach (var panel in panels)
        {
            panel.Value.SetActive(false);
        }

        if (panels.ContainsKey(whatPanel))
        {
            panels[whatPanel].SetActive(true);
        }
    }

    public void ShowMainMenu()
    {
        ShowPanel(UiPanel.MainMenu);
    }
    public void ShowAuthMenu()
    {
        ShowPanel(UiPanel.AuthMenu);
    }

    public void ShowChooseMenu()
    {
        ShowPanel(UiPanel.ChooseMenu);
    }

    public void ShowCreateMenu()
    {
        ShowPanel(UiPanel.CreateLobbyMenu);
    }

    public void ShowJoinMenu()
    {
        ShowPanel(UiPanel.JoinLobbyMenu);
    }

    public void CloseAllPanel()
    {
        foreach (var panel in panels)
        {
            panel.Value.SetActive(false);
        }
    }
}
