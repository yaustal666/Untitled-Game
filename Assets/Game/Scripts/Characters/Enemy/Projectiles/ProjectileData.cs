using UnityEngine;

public enum ProjectileMovementType
{
    Line,
    Wave,
    Homing
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public Sprite Sprite;
    public AnimationClip AnimationClip;

    public GameObject FireEffectPrefab;
    public GameObject HitEffectPrefab;

    public float Lifetime;
    public float Knockback;
    public float BaseDamage;
    public float Speed;

    [Header("Movement Type")]
    public ProjectileMovementType MovementType;
    public float param1;
    public float param2;
    public float param3;

}