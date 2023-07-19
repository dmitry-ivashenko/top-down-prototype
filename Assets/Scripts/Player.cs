using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnAttack;
    
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
        {
            Attack();
        }
    }

    private async Task Attack()
    {
        _animator.SetTrigger("Attack");
        await Task.Delay(250);
        OnAttack?.Invoke();
    }
}
