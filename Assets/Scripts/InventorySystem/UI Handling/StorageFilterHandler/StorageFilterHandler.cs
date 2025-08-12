using System;
using System.Collections.Generic;
using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
using UnityEngine;
using UnityEngine.UIElements;

public class StorageFilterHandler : MonoBehaviour
{
    [SerializeField] private ScrollViewHandler categoryHandler;
    [SerializeField] private Transform categoryParent;

    [SerializeField] private ScrollViewHandler stashHandler;
    [SerializeField] private Transform stashParent;

    [SerializeField] private Transform gridParent;

    [SerializeField] private GameObject prefabCatrgoryOrStash;
    [SerializeField] private List<UICategory> CategoryButtons = new List<UICategory>();

    [SerializeField] private int currentCategoryIndex = 0;
    [SerializeField] private int currentStashIndex = 0;

    public static Action<int> EventCategoryChanged;
    public static Action<int> EventStashChanged;

    [SerializeField] private InventoryGrid grid;
    [Space]
    [Space]
    [Space]
    [SerializeField] bool debug = false;
    [SerializeField] private ItemBase[] items;
    [SerializeField] private DragDropUI prefab;
    private void DebugCreate(ItemBase[] items)
    {
        for (int i = 0; i < 1; i++)
        {
            var newCAtegory = GeneralFactory.CreateCategory(categoryParent, prefabCatrgoryOrStash);
            newCAtegory.CategoryButton.onClick.AddListener(() => { UpdateCategoryUI(newCAtegory.transform.GetSiblingIndex()); });
            categoryHandler.AddButton.transform.SetAsLastSibling();
            CategoryButtons.Add(newCAtegory);
            
            for (int x = 0; x < 3; x++)
            {
                var newStash = GeneralFactory.CreateStash(stashParent, prefabCatrgoryOrStash, currentCategoryIndex);
                newStash.name = "New Stash " + x;
                newStash.Button.onClick.AddListener(() => { UpdateStashChangeUI(newStash.transform.GetSiblingIndex()); });
                CategoryButtons[currentCategoryIndex].AddStash(newStash);
                stashHandler.AddButton.transform.SetAsLastSibling();
              
            }
        }
        for (int z = 0; z < items.Length; z++)
        {
            var item = items[z];
            bool canPlace = grid.TryFindFirstAvailablePosition(item, out Vector2Int pos);
            if (canPlace)
            {
                var drag = GeneralFactory.CreateDragDropUI(gridParent, prefab, item, pos, grid);
           

                CategoryButtons[currentCategoryIndex].ItemStashes[currentStashIndex].ActiveUi.Add(drag);

            }

        }

    }

    private void OnValidate()
    {
        grid = GetComponent<InventoryGrid>();
        categoryParent = transform.parent.parent.parent.GetComponentInChildren<CategoryTag>().GetComponent<Transform>();
        stashParent = transform.parent.parent.parent.GetComponentInChildren<StashTag>().GetComponent<Transform>();

        gridParent = GetComponent<PlayerGridTag>().GetComponent<Transform>();
        categoryHandler?.AddButton.onClick.AddListener(CreateCategory);
        stashHandler?.AddButton?.onClick.AddListener(CreateStash);


    }
    private void Awake()
    {
        grid = GetComponent<InventoryGrid>();
        categoryParent = transform.parent.parent.parent.GetComponentInChildren<CategoryTag>().GetComponent<Transform>();
        stashParent = transform.parent.parent.parent.GetComponentInChildren<StashTag>().GetComponent<Transform>();

        gridParent = GetComponent<PlayerGridTag>().GetComponent<Transform>();


        categoryHandler?.AddButton.onClick.AddListener(CreateCategory);
        stashHandler?.AddButton?.onClick.AddListener(CreateStash);

        if (debug) DebugCreate(items);
    }
    private void CreateCategory()
    {
        var cat = GeneralFactory.CreateCategory(categoryParent, prefabCatrgoryOrStash);
        cat.CategoryButton?.onClick.AddListener(() => { UpdateCategoryUI(cat.transform.GetSiblingIndex()); });
        CategoryButtons.Add(cat);
        categoryHandler.AddButton.transform.SetAsLastSibling();
    }


    private void CreateStash()
    {
        var stash = GeneralFactory.CreateStash(stashParent, prefabCatrgoryOrStash, currentStashIndex);
        stash.Button?.onClick.AddListener(() => { UpdateStashChangeUI(stash.transform.GetSiblingIndex()); });
        CategoryButtons[currentCategoryIndex].AddStash(stash);
        stashHandler.AddButton.transform.SetAsLastSibling();

    }
    private void UpdateStashChangeUI(int newStashIndex)
    {
        currentStashIndex = newStashIndex;
        var stashes = CategoryButtons[currentCategoryIndex].ItemStashes;
        if(stashes.Count == 0) return;

        foreach (var stash in stashes)
        {
            foreach (var DragDropUI in stash.ActiveUi)
            {
                if(stash.ActiveUi.Count == 0) continue;
                if (!DragDropUI.IsDragged && DragDropUI.ItemData.StashIndex == newStashIndex)
                {
                    DragDropUI.gameObject.SetActive(true);
                    grid.MarkSlots(DragDropUI.ItemData, DragDropUI, DragDropUI.ItemData.GridPosition);
                }
                else
                {
                    DragDropUI.gameObject.SetActive(false);
                    grid.MarkSlots(DragDropUI.ItemData, null, DragDropUI.ItemData.GridPosition);
                }
            }   
        }
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
   
}
