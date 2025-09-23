using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField, Range(0.0f, 10.0f)] private float _moveSpeed = 1.0f;
    [SerializeField, Range(0.0f, 10.0f)] private float _moveSpeedMax = 4.0f;
    [SerializeField, Range(0.0f, 10.0f)] private float _jumpForce = 5.0f;
    [SerializeField] private bool _isGround = false;

    [Header("アニメーション関係")]
    [SerializeField] private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, 0f, z).normalized;

        if (move.magnitude > 0f)
        {
            _rb.AddForce(move * _moveSpeed, ForceMode.VelocityChange);
        }

        if(Input.GetButtonDown("Jump")&& _isGround)
        {
            _animator.SetBool("Jump", true);
            _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            _isGround = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rb.linearVelocity;

        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

        if (horizontalVelocity.magnitude > _moveSpeedMax)
        {
            horizontalVelocity = horizontalVelocity.normalized * _moveSpeedMax;
        }

        _rb.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }

}