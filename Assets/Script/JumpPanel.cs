using UnityEngine;

public class JumpPanel : MonoBehaviour
{
    [SerializeField, Tooltip("パネルがプレイヤーを押し上げる力")]
    private float _jumpForce = 10.0f;

    // プレイヤーがジャンプパネルに触れたとき
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーにRigidbodyがあればジャンプさせる
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 下方向の速度をリセットしてから上方向にジャンプさせる
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;

            // 上方向に力を加える
            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            Debug.Log($"{other.name} がジャンプパネルで飛ばされました");
        }
    }
}