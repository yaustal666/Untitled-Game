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
        _hpPercentage.text = (player.Health.HealthPercentage * 100f).ToString() + "%";
    }
}
