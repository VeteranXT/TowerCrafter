using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class Category : MonoBehaviour
{
    private string categoryName;
    private Sprite categoryIcon;
    private List<ItemStash> itemStash = new List<ItemStash>();

    public Category CreateCategory(Transform parent, GameObject prefab, string categoryName = "")
    {
        Category cat =  Instantiate(prefab, parent.transform).GetComponent<Category>();
        cat.categoryName = categoryName;
        return cat;
    }
}

