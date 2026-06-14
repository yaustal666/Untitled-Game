using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionView : MonoBehaviour
{
    [Inject] private LocalizationSystem _loc;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemDescription;

    public void ShowItem(ItemData item)
    {
        _itemIcon.enabled = true;
        _itemIcon.sprite = item.Icon;
        var itemInfo = _loc.GetItemInfo(item.Id);

        _itemDescription.text = itemInfo.Description;
    }

    public void Clear()
    {
        _itemIcon.sprite = null;
        _itemIcon.enabled = false;
        _itemDescription.text = "";
    }
}