using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : ViewBase
{
    [Header("View Refs")]
    public ViewBase OptionsView;
    public GameObject LobbyCam;
    public GameObject MainUI;
    public GameObject InGameUI;
    public Button StartButton;
    public Button OptionsButton;
    public Button ExitButton;

    protected override void OnInit()
    {
        StartButton.onClick.AddListener(() =>
        {
            LobbyCam.SetActive(false);
            MainUI.SetActive(false);
            
            InGameUI.SetActive(true);
            
            GameManager.Instance.StartGame();
        });
        
        OptionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsView.Show();
        });
        
        ExitButton.onClick.AddListener(Application.Quit);
    }

    protected override void OnShow()
    {
        LobbyCam.SetActive(true);
        MainUI.SetActive(true);
        InGameUI.SetActive(false);
    }
}
