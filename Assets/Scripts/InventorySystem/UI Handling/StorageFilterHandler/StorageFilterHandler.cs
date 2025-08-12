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

    [SerializeField] private List<DragDropUI> activeUI = new List<DragDropUI>();
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

    }
    private void CreateCategory()
    {
        var cat = UICategory.CreateCategory(categoryParent, prefabCatrgoryOrStash);
        cat.transform.SetAsLastSibling();
        cat.SetSiblingIndex(transform.transform.GetSiblingIndex());
        CategoryButtons.Add(cat);
    }
    private void CreateStash()
    {
        var stash = UIStash.CreateStash(stashParent, prefabCatrgoryOrStash, currentStashIndex);
        stash.transform.SetAsLastSibling();
        CategoryButtons[currentCategoryIndex].AddStash(stash);
    }
    private void UpdateStashChangeUI(int newStashIndex)
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
