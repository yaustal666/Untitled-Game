using UnityEngine;

public static class GameConstants
{
    public static readonly int Anim_HorizontalMoveDirection = Animator.StringToHash("x");
    public static readonly int Anim_VerticalMoveDirection = Animator.StringToHash("y");
    public static readonly int Anim_Running = Animator.StringToHash("running");

    public static readonly string Input_MoveAction = "Move";
    public static readonly string Input_JumpAction = "Jump";
    public static readonly string Input_DashAction = "Dash";
    public static readonly string Input_AttackAction = "Attack";

    public static readonly float Platformer_DefaultGravity = 4f;

    public static readonly Vector2Int[] roomTraverseOrder =
    {
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1)
    };

}