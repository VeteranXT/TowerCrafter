
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStash : MonoBehaviour
{
    [SerializeField] private TMP_Text stashName;
    [SerializeField] private Image stashIcon;
    [SerializeField] private Button stashButton;
    [SerializeField] private List<DragDropUI> activeUi = new List<DragDropUI>();
    [SerializeField] private int categoryIndex;
    public int CategoryIndex { get { return categoryIndex; }  set { categoryIndex = value; } }
    public List<DragDropUI> DragDropUI { get { return activeUi; } }
    public int CurretStashIndex()
    {
        return transform.GetSiblingIndex();
    }

  
}