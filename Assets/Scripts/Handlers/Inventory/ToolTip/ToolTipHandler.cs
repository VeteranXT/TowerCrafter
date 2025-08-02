// 7/12/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using TMPro;

public class ToolTipHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text textItemName;
    [SerializeField] private TMP_Text textItemDescription;
    [SerializeField] private GameObject toolTipWindow;

    private void Start()
    {
        if (toolTipWindow == null || textItemName == null || textItemDescription == null)
        {
            Debug.LogError("ToolTipHandler is missing references. Please assign all fields in the Inspector.");
            return;
        }

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
                ShowToolTip(data);
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
        if (toolTipWindow != null)
            toolTipWindow.SetActive(false);
    }

    private void ShowToolTip(IInformation data)
    {
        if (textItemDescription == null || textItemName == null)
        {
            Debug.LogError("Tooltip text fields are not assigned.");
            return;
        }
        if (data == null) 
            return;
        textItemDescription.text = data.Description();
        textItemName.text = data.InformationName();
        toolTipWindow.SetActive(true);
    }
}