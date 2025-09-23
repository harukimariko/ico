using UnityEngine;

public class AirPanel : MonoBehaviour
{
    [SerializeField, Tooltip("上昇する力の強さ（小さいほどゆっくり上がる）")]
    private float _liftForce = 5.0f;

    [SerializeField, Tooltip("最大上昇速度")]
    private float _maxUpVelocity = 3.0f;

    // プレイヤーがパネルの上に乗っている間
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 現在の速度
            Vector3 velocity = rb.linearVelocity;

            // 上方向の速度が上限以下なら上昇力を加える
            if (velocity.y < _maxUpVelocity)
            {
                rb.AddForce(Vector3.up * _liftForce, ForceMode.Acceleration);
            }
        }
    }
}
