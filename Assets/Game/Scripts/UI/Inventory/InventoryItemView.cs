using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    public event Action<ItemData, int> ItemChosen;

    public ItemData item;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _highlite;
    [SerializeField] private Image _itemActive;

    public TMP_Text amountText;

    public void SetItem(ItemData itemData, int amount)
    {
        item = itemData;
        _itemIcon.sprite = item.Icon;
        amountText.text = amount.ToString();
    }

    public void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    public void Unchoose()
    {
        _itemActive.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _itemActive.enabled = true;
        ItemChosen.Invoke(item, index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlite.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlite.enabled = false;
    }
}
