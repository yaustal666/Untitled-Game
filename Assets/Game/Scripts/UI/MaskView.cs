using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskView : MonoBehaviour
{
    [Inject] private Mask _maskModel;

    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private MaskCellView _cellPrefab;

    [SerializeField] private List<MaskCellView> _cellViewList = new();
    private MaskColor _currentSelectedColor;

    private List<Vector2Int> _activeConflicts = new List<Vector2Int>();
    private Vector2Int _lastHoveredIndex;

    public int width;
    public int height;

    private void Start()
    {

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var index = i * height + j;
                var cellView = _cellViewList[index];
                cellView.Initialize(new Vector2Int(i, j));

                cellView.OnCellPointerEnter += HandleCellPointerEnter;
                cellView.OnCellPointerExit += HandleCellPointerExit;
                cellView.OnCellPointerClick += HandleCellPointerClick;
            }
        }

        RefreshAllCells();
    }

    public void SetCurrentColor(int colorIndex)
    {
        _currentSelectedColor = (MaskColor)colorIndex;

        if (_lastHoveredIndex != null)
        {
            HandleCellPointerEnter(_lastHoveredIndex);
        }
    }

    private void HandleCellPointerEnter(Vector2Int index)
    {
        _lastHoveredIndex = index;
        ClearActiveConflicts();

        MaskCellValidationResult result = _maskModel.EvaluatePlacement(index, _currentSelectedColor);

        if (result.IsSuccess)
        {
            _cellViewList[index.x * height + index.y].SetHoverPreview();
        }
        else
        {
            foreach (Vector2Int conflictPos in result.Conflicts)
            {
                _cellViewList[conflictPos.x * height + conflictPos.y].SetConflictState(true);
                _activeConflicts.Add(conflictPos);
            }
        }
    }

    private void HandleCellPointerExit(Vector2Int index)
    {
        _cellViewList[index.x * height + index.y].ResetVisual();
        ClearActiveConflicts();
    }

    private void HandleCellPointerClick(Vector2Int index)
    {
        bool successfullyPainted = _maskModel.PaintCell(index, _currentSelectedColor);

        if (successfullyPainted)
        {
            ClearActiveConflicts();
        }
        else
        {
            Debug.LogWarning($"Не удалось покрасить клетку {index}!");
        }
    }

    private void ClearActiveConflicts()
    {
        foreach (Vector2Int pos in _activeConflicts)
        {
            _cellViewList[pos.x * height + pos.y].ResetVisual();
        }
        _activeConflicts.Clear();
    }

    // Полное обновление всей сетки (например, после загрузки игры)
    public void RefreshAllCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Для этого вызовем хелпер, который мы допишем ниже
                UpdateCellFromModel(new Vector2Int(x, y));
            }
        }
    }

    private void UpdateCellFromModel(Vector2Int index)
    {
        // Для работы этого метода добавь в свой класс Mask маленькое свойство-геттер:
        // public int[,] Grid => _grid; 
        // Или метод: public int GetCellRaw(Vector2Int idx) => _grid[idx.x, idx.y];

        // Пример реализации:
        // int val = _maskModel.GetCellRaw(index);
        // _cellViews[index.x, index.y].SetColor(val > 0 ? (MaskColor?)(val - 1) : null);
    }

    private void OnDestroy()
    {
        foreach (var cell in _cellViewList)
        {
            if (cell == null) continue;
            cell.OnCellPointerEnter -= HandleCellPointerEnter;
            cell.OnCellPointerExit -= HandleCellPointerExit;
            cell.OnCellPointerClick -= HandleCellPointerClick;
        }
    }

}
