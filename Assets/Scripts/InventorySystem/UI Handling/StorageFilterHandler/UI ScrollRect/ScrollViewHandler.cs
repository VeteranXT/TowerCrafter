using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrollViewHandler : MonoBehaviour
{
    [SerializeField] private ScrollRect rectView;
    [SerializeField] private Transform content;
    [SerializeField] private Button moveRight;
    [SerializeField] private Button moveLeft;
    [SerializeField] private Button addButton;
    private readonly RectTransform contentRect;
    private RectTransform viewportRect;
    public Button GetAddButtom { get { return addButton; } }
    public Transform ParentContentTransform { get { return content; } }
    private void Start()
    {
        rectView = GetComponent<ScrollRect>(); 
        if (contentRect == null || moveLeft == null || moveRight == null || addButton == null)
        {
            return;
        }
        viewportRect = rectView.viewport;
 
        moveRight.onClick.AddListener(MoveRight);
        moveLeft.onClick.AddListener(MoveLeft);
        addButton.onClick.AddListener(() =>
        { 
            addButton.transform.SetAsLastSibling();
        });
    }

    private void MoveLeft()
    {
        float viewportWidth = viewportRect.rect.width;
        Vector2 newPosition = rectView.content.anchoredPosition;
        newPosition.x += viewportWidth;

        // Clamp the position to ensure we don't scroll past the content's boundaries
        newPosition.x = Mathf.Min(newPosition.x, 0);

        rectView.content.anchoredPosition = newPosition;
        UpdateButtonStates();
    }
    private void UpdateButtonStates()
    {
        float viewportWidth = viewportRect.rect.width;
        float contentWidth = contentRect.rect.width;

        // Disable both buttons if content fits in the viewport
        if (contentWidth <= viewportWidth)
        {
            moveRight.interactable = false;
            moveLeft.interactable = false;
            return;
        }

        // Enable/disable buttons based on scroll position
        float currentX = rectView.content.anchoredPosition.x;
        moveLeft.interactable = currentX < 0;
        moveLeft.interactable = currentX > -(contentWidth - viewportWidth);
    }
    private void MoveRight()
    {
        float viewportWidth = viewportRect.rect.width;
        float contentWidth = contentRect.rect.width;

        Vector2 newPosition = rectView.content.anchoredPosition;
        newPosition.x -= viewportWidth;

        // Clamp the position to ensure we don't scroll past the content's boundaries
        newPosition.x = Mathf.Max(newPosition.x, -(contentWidth - viewportWidth));

        rectView.content.anchoredPosition = newPosition;
        UpdateButtonStates();
    }

}
