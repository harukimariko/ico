using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractor : MonoBehaviour
{
    [Header("吸収対象（プレイヤー）")]
    [SerializeField] private Transform _target;

    [Header("吸収位置オフセット")]
    [SerializeField] private Vector3 _offset = Vector3.zero;

    [Header("吸収エフェクト（任意のParticleSystemを割り当て）")]
    [SerializeField] private ParticleSystem _absorbEffect;

    [Header("パラメータ設定")]
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _absorbDistance = 0.5f;
    [SerializeField] private bool _triggerWithSpace = true;
    [SerializeField] private int _emitCount = 50;

    private ParticleSystem _ps;
    private ParticleSystem.Particle[] _particles;

    void Start()
    {
        _ps = GetComponent<ParticleSystem>();

        var main = _ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startLifetime = 5f;
        main.playOnAwake = false;

        _particles = new ParticleSystem.Particle[main.maxParticles];
    }

    void Update()
    {
        //if (_triggerWithSpace && Input.GetKeyDown(KeyCode.L))
        if (_triggerWithSpace)
        {
            _ps.Emit(_emitCount);
        }

        if (_target == null) return;

        int count = _ps.GetParticles(_particles);
        Vector3 targetPos = _target.position + _offset;
        float dt = Time.deltaTime;

        for (int i = 0; i < count; i++)
        {
            Vector3 dir = targetPos - _particles[i].position;
            float dist = dir.magnitude;

            // プレイヤーに近づいたら吸収処理
            if (dist < _absorbDistance)
            {
                // 吸収エフェクトを再生
                if (_absorbEffect != null)
                {
                    _absorbEffect.transform.position = _particles[i].position;
                    _absorbEffect.Emit(10); // 吸収時に10粒発生（必要に応じて調整）
                }

                _particles[i].remainingLifetime = 0f;
                continue;
            }

            // 目標に直接近づく
            float step = _moveSpeed * dt;
            _particles[i].position = Vector3.MoveTowards(_particles[i].position, targetPos, step);

            // 見た目：距離でサイズ変化
            float t = Mathf.Clamp01(1f - dist / 10f);
            _particles[i].startSize = Mathf.Lerp(1f, 0.2f, t);
        }

        _ps.SetParticles(_particles, count);
    }

    void OnDrawGizmosSelected()
    {
        if (_target == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_target.position + _offset, _absorbDistance);
    }
}
