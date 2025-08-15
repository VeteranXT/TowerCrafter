using System;
using System.Collections.Generic;
using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
using UnityEngine;

public class StorageFilterManager : MonoBehaviour
{
    private ScrollViewHandler categoryScrollViewUI;
    private ScrollViewHandler stashScrollViewUI;
    private CustomGrid grid;
    private GameObject prefabCatrgoryOrStash;
    private List<UICategory> CategoryButtons = new();

    public int currentCategoryIndex = 0;
    public int currentStashIndex = 0;

    public static Action<int> EventCategoryChanged;
    public static Action<int> EventStashChanged;


    public Transform GetCategoryParentContent { get { return categoryScrollViewUI.ParentContentTransform; } }
    public Transform GetStashParentContent { get { return stashScrollViewUI.ParentContentTransform; } }
    private void Awake()
    {
        FindRefrences();

        ///TO DO: 
        //Check if File Exists
        //if exists
        //Load all items
        //Load all Categories and stashes
        //Create all of categoris and stashes
        //Create UI and Place it.
        //if file do not exist
        InitializeStorage();
    }

    #region Initlization
    public void InitializeStorage()
    {
        for (int i = 0; i < 1; i++)
        {
            var categ = GeneralFactory.CreateCategory(prefabCatrgoryOrStash, GetCategoryParentContent);
            categ.GetButton.onClick.AddListener(() => { UpdateCategoryUI(categ.transform.GetSiblingIndex()); });
            for (int x = 0; x < 3; x++)
            {
                var stash=  GeneralFactory.CreateStash(prefabCatrgoryOrStash, GetStashParentContent);
                stash.GetButton.onClick.AddListener(() => { UpdateStashUI(stash.transform.GetSiblingIndex()); });
                categ.AddStash(stash);
            }
        }
    }
    private void FindRefrences()
    {
        //Get root of all things.
        var rootParent = transform.root;
        //Get Category Scrollview
        var cat = rootParent.GetComponentInChildren<CategoryScrollViewTag>().GetComponent<ScrollViewHandler>();
        categoryScrollViewUI = cat.GetComponent<ScrollViewHandler>();
        //Subscribe to on Click
        categoryScrollViewUI.GetAddButtom.onClick.AddListener(() => { CreateCategory(); });


        stashScrollViewUI = rootParent.GetComponentInChildren<StashScrollViewTag>().GetComponent<ScrollViewHandler>();
        stashScrollViewUI.GetAddButtom.onClick.AddListener(() => { CreateStash(); });

        grid = GetComponent<StorageGrid>();
        prefabCatrgoryOrStash = Resources.Load<GameObject>("Prefabs/Prefab_CategoryOrStash");
    }
    #endregion

    #region Factory
    private UICategory CreateCategory()
    {
        UICategory cat = GeneralFactory.CreateCategory(prefabCatrgoryOrStash, GetCategoryParentContent);
        cat.GetButton.onClick.AddListener(() => { UpdateCategoryUI(cat.transform.GetSiblingIndex()); });
        return cat;
    }
    private UIStash CreateStash()
    {
        var stash = GeneralFactory.CreateStash(prefabCatrgoryOrStash, GetStashParentContent);
        stash.GetButton.onClick.AddListener(() => { UpdateStashUI(stash.transform.GetSiblingIndex()); });
        CategoryButtons[currentCategoryIndex].AddStash(stash);
        return stash;
    }
    #endregion

    #region Event Handlers and Helpers
    private void HideStashesButtons(int newCategory)
    {
        foreach (var category in CategoryButtons)
        {
            foreach (var stash in category.ItemStashes)
            {
                if (category.transform.GetSiblingIndex() != newCategory)
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
    }
    private void UpdateCategoryUI(int newCategory)
    {
        currentCategoryIndex = newCategory;
        HideStashesButtons(newCategory);
        EventCategoryChanged?.Invoke(newCategory);
    }
    private void UpdateStashUI(int newStashIndex)
    {
        var stashes = CategoryButtons[currentCategoryIndex].ItemStashes;

        ShowAndHideUI(newStashIndex);
        currentCategoryIndex = newStashIndex;
        EventStashChanged?.Invoke(newStashIndex);
    }
    private void ShowAndHideUI(int newStashIndex)
    {
        var stashes = CategoryButtons[currentCategoryIndex].ItemStashes;


        foreach (var stash in stashes)
        {
            foreach (var DragDropUI in stash.ActiveUI)
            {
                //Check if current item is dragged or it matches to stash
                //Show UI
                if (!DragDropUI.IsDragged && DragDropUI.ItemData.StashIndex == newStashIndex && DragDropUI.ItemData.CategoryIndex == currentCategoryIndex)
                {
                    DragDropUI.gameObject.SetActive(true);
                    grid.MarkSlots(DragDropUI.ItemData, DragDropUI, DragDropUI.ItemData.GridPosition);
                }
                else
                {
                    DragDropUI.gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion
}
