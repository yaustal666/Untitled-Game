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
    [SerializeField] private Image _conflictImage;

    [SerializeField] private Color _emptyColor = Color.white;
    [SerializeField] private Color _redColor = Color.red;
    [SerializeField] private Color _greenColor = Color.green;
    [SerializeField] private Color _purpleColor = new Color(0.5f, 0f, 0.5f);
    [SerializeField] private Color _yellowColor = Color.yellow;

    public Vector2Int Index { get; private set; }

    public void Initialize(Vector2Int index)
    {
        Index = index;
        ResetVisual();
    }

    public void OnPointerEnter(PointerEventData eventData) => OnCellPointerEnter?.Invoke(Index);
    public void OnPointerExit(PointerEventData eventData) => OnCellPointerExit?.Invoke(Index);
    public void OnPointerClick(PointerEventData eventData) => OnCellPointerClick?.Invoke(Index);

    public void SetColor(MaskColor? color)
    {
        if (color == null)
        {
            _cellImage.color = _emptyColor;
        }
        else
        {
            _cellImage.color = color.Value switch
            {
                MaskColor.Red => _redColor,
                MaskColor.Green => _greenColor,
                MaskColor.Purple => _purpleColor,
                MaskColor.Yellow => _yellowColor,
                _ => _emptyColor
            };
        }
    }

    public void SetHoverPreview()
    {

    }

    public void SetConflictState(bool hasConflict)
    {
        _conflictImage.gameObject.SetActive(hasConflict);
    }

    public void ResetVisual()
    {
        _highlightImage.gameObject.SetActive(false);
        _conflictImage.gameObject.SetActive(false);
    }

}
