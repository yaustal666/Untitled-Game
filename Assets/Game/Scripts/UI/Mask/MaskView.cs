using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MaskView : UIWindow
{
    [Inject] private Player _player;

    [SerializeField] private List<MaskCellView> _cellViewList = new();
    [SerializeField] private ColorChooseView _colorChooseView;

    [SerializeField] private Color _permitColot = Color.green;
    [SerializeField] private Color _conflictColor = Color.red;

    [SerializeField] private Color _emptyColor = Color.white;
    [SerializeField] private Color _redColor = Color.red;
    [SerializeField] private Color _greenColor = Color.green;
    [SerializeField] private Color _purpleColor = new Color(0.5f, 0f, 0.5f);
    [SerializeField] private Color _yellowColor = Color.yellow;

    private MaskColor _currentSelectedColor;

    private List<Vector2Int> _activeConflicts = new List<Vector2Int>();

    public int width;
    public int height;
    private CancellationTokenSource _hoverCts;

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

        _colorChooseView.ColorChosen += SetCurrentColor;
    }

    public void SetCurrentColor(MaskColor color)
    {
        Debug.Log("Color Chosen - " + color.ToString());
        _currentSelectedColor = color;
    }

    private void HandleCellPointerEnter(Vector2Int index)
    {
        _hoverCts?.Cancel();
        _hoverCts?.Dispose();
        _hoverCts = new CancellationTokenSource();
        OnHover(index, _hoverCts.Token).Forget();
    }

    private async UniTask OnHover(Vector2Int index, CancellationToken token)
    {
        bool isCancelled = await UniTask.Delay(300, ignoreTimeScale: true, cancellationToken: token).SuppressCancellationThrow();
        if (isCancelled) return;

        ClearActiveConflicts();

        MaskCellValidationResult result = _player.Mask.EvaluatePlacement(index, _currentSelectedColor);

        if (result.IsSuccess)
        {
            _cellViewList[index.x * height + index.y].Highlite(_permitColot);
        }
        else
        {
            foreach (Vector2Int conflictPos in result.Conflicts)
            {
                _cellViewList[conflictPos.x * height + conflictPos.y].Highlite(_conflictColor);
                _activeConflicts.Add(conflictPos);
            }
        }
    }

    private void HandleCellPointerExit(Vector2Int index)
    {
        _hoverCts?.Cancel();
        _hoverCts?.Dispose();
        _hoverCts = null;
        _cellViewList[index.x * height + index.y].ResetVisual();
        ClearActiveConflicts();
    }

    private void HandleCellPointerClick(Vector2Int index)
    {
        bool successfullyPainted = _player.Mask.PaintCell(index, _currentSelectedColor);

        if (successfullyPainted)
        {
            ClearActiveConflicts();
            _cellViewList[index.x * height + index.y].SetColor(MaskColorToColor(_currentSelectedColor));
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

    private void OnDestroy()
    {
        _hoverCts?.Cancel();
        _hoverCts?.Dispose();
        foreach (var cell in _cellViewList)
        {
            if (cell == null) continue;
            cell.OnCellPointerEnter -= HandleCellPointerEnter;
            cell.OnCellPointerExit -= HandleCellPointerExit;
            cell.OnCellPointerClick -= HandleCellPointerClick;
        }
    }

    public Color MaskColorToColor(MaskColor maskColor)
    {
        return maskColor switch
        {
            MaskColor.Green => _greenColor,
            MaskColor.Red => _redColor,
            MaskColor.Yellow => _yellowColor,
            MaskColor.Purple => _purpleColor,
            _ => throw new KeyNotFoundException()
        };
    }

    [ContextMenu("debugPrint")]
    public void PrintDebug()
    {
        _player.Mask.PrintDebug();
    }
}
