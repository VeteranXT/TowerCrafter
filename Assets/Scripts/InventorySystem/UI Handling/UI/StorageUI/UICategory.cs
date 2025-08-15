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

    private List<UIStash> buttonStashes = new List<UIStash>();
    public List<UIStash> ItemStashes {  get { return buttonStashes; }  set { buttonStashes = value; } }
    public Button GetButton { get { return categoryButton; } }
    
    public void AddStash(UIStash stash)
    {
        buttonStashes.Add(stash);
    }
    public void SetSiblingIndex(int index)
    {
        this.transform.SetSiblingIndex(index);
    }

    private void OnDestroy()
    {
        
    }
}

