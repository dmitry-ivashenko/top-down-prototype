using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [FormerlySerializedAs("_skins")]
    public List<GameObject> Skins;
    public float Speed = 1f;
    public float AttackDistance = 2f;
    
    private GameObject _player;
    private Animator _animator;

    private static int _skinIndex = new System.Random().Next(0, 1000);

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Skins.ForEach(skin => skin.SetActive(false));
        Skins[_skinIndex++ % Skins.Count].SetActive(true);
    }

    public void Init(GameObject player)
    {
        _player = player;
    }

    private void Update()
    {
        if (_player == null) return;

        transform.LookAt(_player.transform);
        
        var distance = _player.transform.position - transform.position;
        if (distance.magnitude > AttackDistance)
        {
            PlayMoveAnimation();
            transform.position += distance.normalized * (Time.deltaTime * Speed);
        }
        else
        {
            PlayAttackAnimation();
        }
    }

    public async Task PlayDieAnimation()
    {
        _animator.SetTrigger("Die");
        await Task.Delay(500);
        Destroy(gameObject);
    }

    private void PlayMoveAnimation()
    {
        _animator.SetBool("Walking", true);
        _animator.SetBool("Attacking", false);
    }

    private void PlayAttackAnimation()
    {
        _animator.SetBool("Walking", false);
        _animator.SetBool("Attacking", true);
    }
}
