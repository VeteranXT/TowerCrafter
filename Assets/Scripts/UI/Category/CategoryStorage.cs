using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CategoryStorage : MonoBehaviour
{
    [SerializeField] private TMP_Text categoryName;
    [SerializeField] private Image categoryIcon;
    [SerializeField] private Button categoryButton;
    public  event Action<int> EventCategoryClicked;

    private List<StashStorage> itemStash = new List<StashStorage>();
    public List<StashStorage> ItemStashes {  get { return itemStash; }  set { itemStash = value; } }

    private void Start()
    {
        categoryButton = GetComponent<Button>();
        categoryButton.onClick?.AddListener(() => { EventCategoryClicked?.Invoke (transform.GetSiblingIndex()); });
    }
    public static CategoryStorage CreateCategory(Transform parent, GameObject prefab, string categoryName = "")
    {
        var cat =  Instantiate(prefab, parent.transform).AddComponent<CategoryStorage>();

        return cat;
    }
    public void AddStash(StashStorage stash)
    {
        itemStash.Add(stash);
    }
    public void SetSiblingIndex(int index)
    {
        this.transform.SetSiblingIndex(index);
    }

    private void OnDestroy()
    {
        
    }
}

