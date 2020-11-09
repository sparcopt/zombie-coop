using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashSystem : MonoBehaviour
{
    private float cash;
    public int InitialCash = 0;
    public Text CashText;
    public float Cash
    {
        get { return cash; }
        set 
        {
            cash = value;
            UpdateUI();
        }
    }

    void Start()
    {
        var inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        CashText = inGameUITransform.Find("Cash").GetComponent<Text>();
        
        cash = InitialCash;
    }

    // Update is called once per frame
    private void UpdateUI()
    {
        CashText.text = "Cash: " + cash + "$";
    }
}
