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
    public Sprite sprite;
    public AnimationClip animationClip;

    public GameObject fireEffectPrefab;
    public GameObject hitEffectPrefab;

    public float lifetime;
    public float knockback;
    public float baseDamage;
    public float speed;

    [Header("Movement type and generic params")]
    public ProjectileMovementType movementType;
    public float param1;
    public float param2;
    public float param3;

}