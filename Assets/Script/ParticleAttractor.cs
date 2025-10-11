using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractor : MonoBehaviour
{
    [Header("�z���Ώہi�v���C���[�j")]
    [SerializeField] private Transform _target;

    [Header("�z���ʒu�I�t�Z�b�g")]
    [SerializeField] private Vector3 _offset = Vector3.zero;

    [Header("�z���G�t�F�N�g�i�C�ӂ�ParticleSystem�����蓖�āj")]
    [SerializeField] private ParticleSystem _absorbEffect;

    [Header("�p�����[�^�ݒ�")]
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
        if (_triggerWithSpace && Input.GetKeyDown(KeyCode.L))
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

            // �v���C���[�ɋ߂Â�����z������
            if (dist < _absorbDistance)
            {
                // �z���G�t�F�N�g���Đ�
                if (_absorbEffect != null)
                {
                    _absorbEffect.transform.position = _particles[i].position;
                    _absorbEffect.Emit(10); // �z������10�������i�K�v�ɉ����Ē����j
                }

                _particles[i].remainingLifetime = 0f;
                continue;
            }

            // �ڕW�ɒ��ڋ߂Â�
            float step = _moveSpeed * dt;
            _particles[i].position = Vector3.MoveTowards(_particles[i].position, targetPos, step);

            // �����ځF�����ŃT�C�Y�ω�
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
