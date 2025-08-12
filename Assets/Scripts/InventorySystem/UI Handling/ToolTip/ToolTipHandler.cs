// 7/12/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using TMPro;

public class ToolTipHandler : MonoBehaviour
{
    [SerializeField] UIToolTip toolTipWindow;

    private void Start()
    {
        DragDropUI.OnToolTipEvent += HandleTooltips;
    }

    private void OnDestroy()
    {
        DragDropUI.OnToolTipEvent -= HandleTooltips; // Unsubscribe to avoid memory leaks
    }

    public void HandleTooltips(IInformation data)
    {
        if (data != null)
        {
            if (toolTipWindow != null)
            {
                ShowToolTip(data);
            }
            else
                Debug.LogError("Tooltip window is not set in the Inspector.");
        }
        else
        {
          
            HideToolTip();
        }
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    private void ShowToolTip(IInformation data)
    {
        toolTipWindow.text.text = data.InformationName();
        toolTipWindow.text.text = data.Description();
        toolTipWindow.gameObject.SetActive(true);
    }
}