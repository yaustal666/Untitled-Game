using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaskCellView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<Vector2Int> OnCellPointerEnter;
    public event Action<Vector2Int> OnCellPointerExit;
    public event Action<Vector2Int> OnCellPointerClick;

    [SerializeField] private Image _cellImage;
    [SerializeField] private Image _highlightImage;
    public Vector2Int Index { get; private set; }

    public void Initialize(Vector2Int index)
    {
        Index = index;
        ResetVisual();
    }

    public void OnPointerEnter(PointerEventData eventData) => OnCellPointerEnter?.Invoke(Index);
    public void OnPointerExit(PointerEventData eventData) => OnCellPointerExit?.Invoke(Index);
    public void OnPointerClick(PointerEventData eventData) => OnCellPointerClick?.Invoke(Index);

    public void SetColor(Color color)
    {
        _cellImage.color = color;
    }

    public void Highlite(Color color)
    {
        _highlightImage.color = color;
        _highlightImage.enabled = true;
    }

    public void ResetVisual()
    {
        _highlightImage.enabled = false;
    }

}
