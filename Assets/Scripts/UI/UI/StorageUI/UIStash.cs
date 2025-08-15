
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStash : BaseMonoBehaviour
{
    [SerializeField] private TMP_Text stashName;
    [SerializeField] private Image stashIcon;
    [SerializeField] private Button stashButton;
    [SerializeField] private List<DragDropUI> activeUi = new();
    public List<DragDropUI> ActiveUI { get { return activeUi; } }

    public Button GetButton { get { return stashButton; } }
    public int CurretStashIndex()
    {
        return transform.GetSiblingIndex();
    }

  
}