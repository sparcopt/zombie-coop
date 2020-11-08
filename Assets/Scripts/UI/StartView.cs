using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : ViewBase
{
    [Header("View Refs")]
    public ViewBase OptionsView;
    public GameObject LobbyCam;
    public GameObject Player;
    public Button StartButton;
    public Button OptionsButton;
    public Button ExitButton;

    protected override void OnInit()
    {
        StartButton.onClick.AddListener(() =>
        {
            print("START");
        });
        
        OptionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsView.Show();
        });
        
        ExitButton.onClick.AddListener(Application.Quit);
    }
}
