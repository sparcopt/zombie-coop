using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewInitializer : MonoBehaviour
{
    public ViewBase EntryView;

    private void Awake()
    {
        var views = GameObject.FindObjectsOfType<ViewBase>();

        foreach (var view in views)
        {
            view.ViewObject.SetActive(true);

            if (view == EntryView)
            {
                continue;
            }
            
            view.ViewObject.SetActive(false);
        }
    }
}
