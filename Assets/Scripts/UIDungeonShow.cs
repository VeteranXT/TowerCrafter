
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIDungeonShow : MonoBehaviour
{
    [SerializeField] private Image dungeonStatusImage;
    [SerializeField] private Dungeon dungeon;
    [SerializeField] TMP_Text UItextTMP;

    //[Button]
    public void ShowDungeon()
    {
        UItextTMP.text = dungeon.GetDungeonDescription();
    }


}

