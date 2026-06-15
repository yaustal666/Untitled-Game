using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorChooseView : MonoBehaviour
{
    public event Action<MaskColor> ColorChosen;
    [SerializeField] private List<ColorChooseButton> _buttonList;

    private void Awake()
    {
        foreach (var button in _buttonList)
        {
            button.ColorChosen += OnChooseColor;
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttonList)
        {
            button.ColorChosen -= OnChooseColor;
        }
    }

    private void OnChooseColor(MaskColor color)
    {
        ColorChosen?.Invoke(color);
    }
}