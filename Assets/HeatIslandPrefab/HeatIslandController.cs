using UnityEngine;

public class HeatIslandController : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField] private float _radiusIsland = 5f;
    [SerializeField] private float _radiusVelocity = 0.1f;

    [Header("パーティクルシステム")]
    [SerializeField] private ParticleSystem _particleSystem;

    [Header("可視化")]
    [SerializeField] private Color _gizmoColor = Color.red;
    [SerializeField] private GameObject _spherePrefab;


    public float _radius
    {
        get => _radiusIsland;
        set
        {
            _radiusIsland = Mathf.Max(0f, value);
            UpdateParticleShape();
            UpdateSphere();
        }
    }

    private void OnValidate()
    {
        UpdateParticleShape();
        UpdateSphere();
    }

    private void UpdateParticleShape()
    {
        if (_particleSystem == null) return;
        var shape = _particleSystem.shape;
        shape.radius = _radiusIsland;
    }

    private void UpdateSphere()
    {
        if (_spherePrefab == null) return;
        _spherePrefab.transform.localScale = new Vector3(_radiusIsland, _radiusIsland, _radiusIsland);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _radiusIsland);
    }

    private void Update()
    {
        _radius += _radiusVelocity;
    }
}