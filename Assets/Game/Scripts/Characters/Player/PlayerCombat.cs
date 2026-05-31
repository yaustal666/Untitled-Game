using Reflex.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Inject] private Player _player;
    [Inject] private InputReader _inputReader;

    private Animator animator;

    [SerializeField] private float comboWindowTime = 0.5f;
    [SerializeField] private int maxComboSteps = 3;

    [SerializeField] private List<Attack> attackList = new List<Attack>();
    private Dictionary<string, Attack> attackDictionary = new Dictionary<string, Attack>();

    private int currentComboStep = 0;
    private float comboInputTimer = 0f;

    private bool isAttacking = false;
    private bool canCombo = false;
    private bool canAttack = true;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        foreach (var attack in attackList)
        {
            attackDictionary[attack.AttackName] = attack;
            attack.OnHit += DealDamage;
            attack.ResetAllHitboxes();
        }

        _inputReader.AttackPressed += OnAttackPressed;
    }

    private void OnAttackPressed()
    {
        if (canAttack)
        {
            PerformAttack();
        }
    }

    private void DealDamage(string attackName, IDamagable damagable)
    {
        damagable.TakeDamage(10f);
    }
    void Update()
    {
        if (!isAttacking && Time.time > comboInputTimer)
        {
            currentComboStep = 0;
            canCombo = false;
        }
    }

    private void PerformAttack()
    {
        if (canCombo)
        {
            currentComboStep++;
        }

        if (currentComboStep >= maxComboSteps)
        {
            currentComboStep = 0;
        }

        animator.SetInteger("combo", currentComboStep);
        animator.SetTrigger("Attack");
        isAttacking = true;
    }

    public void StartAttack()
    {
        canAttack = false;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void EnableComboWindow()
    {
        canAttack = true;
        canCombo = true;
        comboInputTimer = Time.time + comboWindowTime;
    }

    private void OnDisable()
    {
        _inputReader.AttackPressed -= OnAttackPressed;
    }
}