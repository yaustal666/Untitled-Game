using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionView : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemDescription;

    public void ShowItem(ItemData item)
    {
        _itemIcon.enabled = true;
        _itemIcon.sprite = item.Icon;
        _itemDescription.text = item.name;
    }
}