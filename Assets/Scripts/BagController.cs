using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagController : MonoBehaviour
{
    private List<Category> categories = new List<Category>();
    public List<Button> controllers = new List<Button>();


    private void Start()
    {
         GetComponentsInChildren(controllers);
    }

}