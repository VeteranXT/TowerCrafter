
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StashStorage : MonoBehaviour
{
    private TMP_Text stashName;
    private Image stashIcon;
    private Button stashButton;
    public List<DragDropUI> DragDropUI = new List<DragDropUI>();
    private int categoryIndex;
    public event Action<int> EventStashClicked;
    public int CategoryIndex { get { return categoryIndex; }  set { categoryIndex = value; } }
    public int CurretStashIndex()
    {
        return transform.GetSiblingIndex();
    }
    private void Start()
    {
        stashButton = GetComponent<Button>();
        if (stashButton != null)
        {
            stashButton?.onClick.AddListener(() => { EventStashClicked?.Invoke(transform.GetSiblingIndex()); });
        }
    }
    public static StashStorage CreateStash(Transform parent, GameObject prefab, int categoryIndex, string name = "")
    {
        var storage =  Instantiate(prefab, parent).AddComponent<StashStorage>(); 
        storage.categoryIndex = categoryIndex;
        
        return storage;
    }
}