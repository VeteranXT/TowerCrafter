using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UICategory : MonoBehaviour
{
    [SerializeField] private TMP_Text categoryName;
    [SerializeField] private Image categoryIcon;
    [SerializeField] private Button categoryButton;

    private List<UIStash> itemStash = new List<UIStash>();
    public List<UIStash> ItemStashes {  get { return itemStash; }  set { itemStash = value; } }

    public static UICategory CreateCategory(Transform parent, GameObject prefab, string categoryName = "")
    {
        var cat =  Instantiate(prefab, parent.transform).AddComponent<UICategory>();

        return cat;
    }
    public void AddStash(UIStash stash)
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

