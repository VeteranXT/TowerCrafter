using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UICategory : BaseMonoBehaviour
{
    [SerializeField] private TMP_Text categoryName;
    [SerializeField] private Image categoryIcon;
    [SerializeField] private Button categoryButton;

    private List<UIStash> stashButtons = new();
    public List<UIStash> ItemStashes {  get { return stashButtons; }  set { stashButtons = value; } }
    public Button GetButton { get { return categoryButton; } }
    
    public void AddStash(UIStash stash)
    {
        stashButtons.Add(stash);
    }
    public void SetSiblingIndex(int index)
    {
        this.transform.SetSiblingIndex(index);
    }
}

