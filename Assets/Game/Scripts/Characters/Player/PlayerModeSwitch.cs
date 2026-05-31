using Reflex.Attributes;
using UnityEngine;

public class PlayerModeSwitch : MonoBehaviour
{
    [Inject] private Player _player;

    private TopDownMovement _topDownMovement;
    private PlatformerMovement _platformerMovement;

    private void Awake()
    {
        _topDownMovement = GetComponent<TopDownMovement>();
        _platformerMovement = GetComponent<PlatformerMovement>();
    }

    private void Start()
    {
        _player.PlayerChangedMode += SwitchMode;
    }

    private void SwitchMode(PlayerMode mode)
    {
        Debug.Log("SWITCH MODE");
        switch (mode)
        {
            case PlayerMode.Platformer:
                _topDownMovement.SetEnabled(false);
                _platformerMovement.SetEnabled(true);
                break;

            case PlayerMode.TopDown:
                _topDownMovement.SetEnabled(true);
                _platformerMovement.SetEnabled(false);
                break;

            default:
                break;
        }
    }

    private void OnDisable()
    {
        _player.PlayerChangedMode -= SwitchMode;
    }
}