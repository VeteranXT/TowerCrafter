using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UITabEditor : BaseMonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private InputField nameField;
    [SerializeField] private Dropdown categoryDropDown;
    [SerializeField] private List<Toggle> tabColors;
    [SerializeField] private List<Toggle> tabIcons;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmCategoyButton;
    [SerializeField] private Button confirmStashButton;
}