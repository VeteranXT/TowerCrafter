using System;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnValidate()
    {
        //categoryHandler = 
        categoryParent = GetComponentInChildren<CategoryTag>().GetComponent<Transform>();
        stashParent = GetComponentInChildren<StashTag>().GetComponent<Transform>();
        gridParent = GetComponentInChildren<PlayerGridTag>().GetComponent<Transform>();

   
    }
    private void Start()
    {

        categoryParent = GetComponentInChildren<CategoryTag>().GetComponent<Transform>();
        stashParent = GetComponentInChildren<StashTag>().GetComponent<Transform>();
        gridParent = GetComponentInChildren<PlayerGridTag>().GetComponent<Transform>();
        categoryHandler.GetAddButtom.onClick.AddListener(() =>{ CreateCategory(); });

    }
    private UICategory CreateCategory()
    {
        var cat = GeneralFactory.CreateCategory(categoryParent, prefabCatrgoryOrStash);
        cat.transform.SetAsLastSibling();
        var simblingIndex = cat.transform.GetSiblingIndex();

        cat.GetButton.onClick.AddListener(() => { UpdateCategoryUI(simblingIndex); });
        CategoryButtons.Add(cat);
        return cat;
    }
    private UIStash CreateStash()
    {
        var stash = GeneralFactory.CreateStash(stashParent, prefabCatrgoryOrStash);
        stash.transform.SetAsLastSibling();
        CategoryButtons[currentCategoryIndex].AddStash(stash);
        return stash;
    }

    private void UpdateStashButtonsVisiblity(int newStashIndex)
    {
        var stashes = CategoryButtons[currentCategoryIndex].ItemStashes;

        foreach (var stash in stashes)
        {
            foreach (var DragDropUI in stash.DragDropUI)
            {
                //Check if current item is dragged or it matches to stash
                //Show UI
                if (!DragDropUI.IsDragged && DragDropUI.ItemData.StashIndex == newStashIndex)
                {
                    DragDropUI.gameObject.SetActive(true);
                }
                else
                {

                    DragDropUI.gameObject.SetActive(false);

                }
            }
        }
        currentCategoryIndex = newStashIndex;
        EventStashChanged?.Invoke(newStashIndex);
    }
    private void UpdateCategoryUI(int newCategory)
    {
        currentCategoryIndex = newCategory;
        HideStashesButtons(newCategory);
        EventCategoryChanged?.Invoke(newCategory);
    }
    private void ShowOrHideUI(int newStashIndex)
    {
        currentStashIndex = newStashIndex;
        foreach (var category in CategoryButtons)
        {
            if (category.ItemStashes.Count == 0) continue;
            foreach (UIStash stash in category.ItemStashes)
            {
                foreach (var UI in stash.DragDropUI)
                {
                    var item = UI.ItemData;
                    var itemCategoryIndex = item.CategoryIndex;
                    var itemStashIndex = item.StashIndex;
                   
                    if (!UI.IsDragged 
                        && itemStashIndex != newStashIndex 
                        && itemCategoryIndex != currentCategoryIndex )
                    {
                        Debug.Log("Hiding  item: " + item.ItemName);
                        UI.gameObject.SetActive(false);
                        //grid.MarkSlots(item, null, item.GridPosition);

                    }
                    else
                    {
                        UI.gameObject.SetActive(true);
                        //grid.MarkSlots(item, UI, item.GridPosition);
                    }
                }
            }
        }
    }
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


  

}
