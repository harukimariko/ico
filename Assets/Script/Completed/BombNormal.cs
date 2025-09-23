using UnityEngine;

public class BombNormal : MonoBehaviour
{
    [Header("爆発パラメータ")]
    [SerializeField, Tooltip("爆発の範囲")]
    private float explosionRadius = 5f;

    [SerializeField, Tooltip("爆発の威力")]
    private float explosionForce = 700f;

    [SerializeField, Tooltip("爆発時に上方向に追加される力")]
    private float upwardsModifier = 1.0f;

    [Header("エフェクト")]
    [SerializeField, Tooltip("爆発エフェクトのPrefab")]
    private GameObject explosionEffectPrefab;

    [Header("爆発後の待機時間")]
    [SerializeField, Tooltip("爆発後に自動削除されるまでの時間")]
    private float destroyDelay = 2f;

    private bool hasExploded = false; // 二重爆発防止

    /// <summary>
    /// 何かに衝突したら爆発
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded&&collision.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    private void Explode()
    {
        hasExploded = true;

        // 爆発エフェクト生成
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // OverlapSphereで範囲内のColliderを取得
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Rigidbodyを持つオブジェクトに爆発力を加える
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }

            // もしHPを持っているならダメージ処理（任意）
            var damageable = nearbyObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(50); // 例: 50ダメージ
            }
        }

        // 爆弾オブジェクトを削除
        Destroy(gameObject, destroyDelay);
    }

    /// <summary>
    /// Sceneビューで爆発範囲を確認できる
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
