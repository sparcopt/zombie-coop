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
        cash = InitialCash;
    }

    // Update is called once per frame
    private void UpdateUI()
    {
        CashText.text = "Cash: " + cash + "$";
    }
}
