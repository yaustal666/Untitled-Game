using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animation _animation;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<Animation>();
    }

    public void Initialize(ProjectileData data)
    {
        spriteRenderer.sprite = data.sprite;
        _animation.clip = data.animationClip;
        _animation.Play();
    }

    public void PlayHitEffect(ProjectileData data, Vector3 position)
    {
        if (data.hitEffectPrefab != null)
        {
            Instantiate(data.hitEffectPrefab, position, Quaternion.identity);
        }
    }
}
