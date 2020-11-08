using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsView : ViewBase
{
    [Header("View Refs")]
    public ViewBase StartView;

    public Button BackToStartButton;

    protected override void OnInit()
    {
        BackToStartButton.onClick.AddListener(() =>
        {
            Hide();
            StartView.Show();
        });
    }
}
