using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorChooseButton : MonoBehaviour, IPointerClickHandler
{
    public event Action<MaskColor> ColorChosen;
    public MaskColor Color;

    public void OnPointerClick(PointerEventData eventData)
    {
        ColorChosen?.Invoke(Color);
    }
}