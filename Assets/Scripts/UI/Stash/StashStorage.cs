
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StashStorage : MonoBehaviour
{
    [SerializeField] private TMP_Text stashName;
    [SerializeField] private Image stashIcon;
    [SerializeField] private Button stashButton;
    [SerializeField] private List<DragDropUI> activeUi = new List<DragDropUI>();
    private int categoryIndex;
    public event Action<int> EventStashClicked;
    public int CategoryIndex { get { return categoryIndex; }  set { categoryIndex = value; } }
    public List<DragDropUI> DragDropUI { get { return activeUi; } }
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