using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StorageFilterHandler : MonoBehaviour
{
    [SerializeField] private Transform categoryParent;
    [SerializeField] private Transform stashParent;
    [SerializeField] private InventoryGrid inventoryGrid;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject categoryStashPrefab;

    [SerializeField] private List<DragDropUI> activeUI = new List<DragDropUI>();
    [SerializeField] private List<CategoryStorage> CategoryButtons = new List<CategoryStorage>();

    [SerializeField] private int currentCategoryIndex = 0;
    [SerializeField] private int currentStashIndex = 0;

    public static Action<int> EventCategoryChanged;
    public static Action<int> EventStashChanged;

    private void Start()
    {
        inventoryGrid = GetComponentInChildren<InventoryGrid>();
        ScrollViewHandler.EventCategoryCreated += CreateCategory;
        ScrollViewHandler.EventStashCreated += CreateStash;
    }
    private void CreateCategory()
    {
        var cat = CategoryStorage.CreateCategory(categoryParent, categoryStashPrefab);
        cat.transform.SetAsLastSibling();
        cat.SetSiblingIndex(transform.transform.GetSiblingIndex());
        cat.EventCategoryClicked += UpdateCategoryUI;
        CategoryButtons.Add(cat);
    }
    private void CreateStash()
    {
        var stash = StashStorage.CreateStash(stashParent, categoryStashPrefab, currentStashIndex);
        stash.transform.SetAsLastSibling();
        stash.EventStashClicked += UpdateStashChangeUI;
        CategoryButtons[currentCategoryIndex].AddStash(stash);
    }
    private void UpdateStashChangeUI(int newStashIndex)
    {
        var stashes = CategoryButtons[currentCategoryIndex].ItemStashes;

        foreach (var stash in stashes)
        {
            foreach (var UI in stash.DragDropUI)
            {
                //Check if current UI ItemName is in curret stash and we are not Draging item
                if(UI.ItemData.StashIndex != newStashIndex && !UI.IsDragged)
                {

                    UI.gameObject.SetActive(false);
                    GridUtils.MarkSlots(UI.ItemData, null, UI.ItemData.GridPosition, inventoryGrid);
                }
                else
                {
                    //This UI is currently Dragged or matches current Stash Index
                    UI.gameObject.SetActive(true);
                    GridUtils.MarkSlots(UI.ItemData, UI, UI.ItemData.GridPosition, inventoryGrid);
                }
            }
        }
        currentCategoryIndex = newStashIndex;
        EventStashChanged?.Invoke(newStashIndex);
    }
    private void UpdateCategoryUI(int NewCategoryIndex)
    {
        currentStashIndex = 0;
        
        foreach (var category in CategoryButtons)
        {
            foreach (var stash in category.ItemStashes)
            {
                if (category.transform.GetSiblingIndex() != NewCategoryIndex)
                {
                    //Hide all stashes form older Category
                    stash.gameObject.SetActive(false);
                }
                else
                {
                    //Show stashes to new Category
                    stash.gameObject.SetActive(true);
                }
            }
           
        }
        currentCategoryIndex = NewCategoryIndex;
        UpdateStashChangeUI(currentStashIndex);
        EventCategoryChanged?.Invoke(currentCategoryIndex);



    }

    public void ChangeStashCategory()
    {

    }
   
}
