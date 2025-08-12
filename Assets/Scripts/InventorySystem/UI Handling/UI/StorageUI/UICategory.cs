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

    private List<UIStash> stashe = new List<UIStash>();
    public List<UIStash> ItemStashes {  get { return stashe; }  set { stashe = value; } }
    public Button CategoryButton { get { return categoryButton; } }
    private void Awake()
    {
        categoryName = GetComponent<TMP_Text>();
        categoryIcon = GetComponent<Image>();
        categoryButton = GetComponent<Button>();
    }
    public void AddStash(UIStash stash)
    {
        stashe.Add(stash);
    }
    public void SetSiblingIndex(int index)
    {
        this.transform.SetSiblingIndex(index);
    }
}

