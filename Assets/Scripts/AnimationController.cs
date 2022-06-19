using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _attackAnimationEffect;
    [SerializeField] private GameObject _weapon;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Plant _targetPlant;

    private void Start()
    {
        _weapon.SetActive(false);
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RunAnimationController();

        ReapAnimationController();
    }

    private void RunAnimationController()
    {
        if(_rigidbody.velocity.magnitude > 0.1)
        {
            _animator.SetBool("Run", true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }
    }

    private void ReapAnimationController()
    {
        if (_targetPlant != null)
        {
            if (_targetPlant.ReadyForCutting)
            {
                _animator.SetBool("Reap", true);
                _weapon.SetActive(true);
            }
            else
            {
                StopReap();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("Plant"))
        {
            if (collision.gameObject.TryGetComponent<Plant>(out _targetPlant) == false)
            { 
                Debug.LogWarning("You don't add Plant.cs on object with tag Plant!");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Plant"))
        {
            StopReap();
        }
    }

    private void StopReap()
    {
        _targetPlant = null;
        _animator.SetBool("Reap", false);
        _weapon.SetActive(false);
    }

    //For Event in Reap animation
    private void Attack()
    {
        if (_targetPlant != null)
        {
            _targetPlant.TakeDamage();
            _attackAnimationEffect.Play();
        }
    }
}
