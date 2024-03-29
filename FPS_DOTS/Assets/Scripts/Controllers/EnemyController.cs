using BaseTool;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected float _lifePoints = 10;

    [SerializeField]
    protected float _moveSpeed = 1;

    [SerializeField]
    protected float _attackRange = 2;

    [SerializeField]
    protected Animator _animator;

    public PlayerController Target;

    private bool _isDead = false;

    private static int _moveSpeedToHash = Animator.StringToHash("MoveSpeed");
    private static int _attackToHash = Animator.StringToHash("Attack");
    private static int _deathToHash = Animator.StringToHash("Death");

    private void Awake() => Injector.Process(this);

    private void Attack()
    {
        _animator.SetTrigger(_attackToHash);
    }

    internal void IsMoving(bool isMoving)
    {
        _animator.SetFloat(_moveSpeedToHash, isMoving ? 1 : 0);
    }

    public void TakeDamages(double damages)
    {
        if (_isDead) return;

        _lifePoints -= (int)damages;
        if (_lifePoints < 0)
        {
            _isDead = true;
            _animator.SetBool(_deathToHash, true);
            Destroy(gameObject, 5);
        }
    }
}
