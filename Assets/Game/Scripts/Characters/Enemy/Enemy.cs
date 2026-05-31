using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    protected SpriteRenderer _sr;
    protected Animator _anim;
    protected Hurtbox _hurtbox;

    protected void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _hurtbox = GetComponentInChildren<Hurtbox>();
        _hurtbox.Owner = this;
    }

    protected virtual void OnDeath()
    {
        //SendLootToPlayer();
    }

    public virtual void TakeDamage(float value)
    {
    }

    //public void SendLootToPlayer()
    //{
    //    Debug.Log("Send Call");
    //    //var player = Player.Instance;
    //    //foreach (var item in loot.entries)
    //    //{
    //    //    if (Chance.Roll(item.DropChance))
    //    //    {
    //    //        //player.GetItem(item.itemID, item.Amount);
    //    //    }
    //    //}
    //}
}