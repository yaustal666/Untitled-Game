using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [Inject] private Player player;

    public Image _hpBar;
    public TMP_Text _hpPercentage;
    public RectTransform _hpBarShape;
    public RectTransform _hpBarBackground;

    private void Update()
    {
        _hpBar.fillAmount = player.Health.HealthPercentage;
        _hpBarShape.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.Health.MaxHealth * 5);
        _hpBarBackground.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.Health.MaxHealth * 5);
        _hpPercentage.text = (player.Health.HealthPercentage * 100f).ToString() + "%";
    }
}
