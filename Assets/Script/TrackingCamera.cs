using Unity.VisualScripting;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    [Header("制御パラメータ")]
    [SerializeField] private Transform _owner; // 自分自身
    [SerializeField] private Transform _target; // ターゲット
    [SerializeField] private Vector3 _offset = Vector3.zero;    // オフセット値
    [SerializeField, Range(1.0f, 20.0f)] private float _distance = 10.0f; // 距離
    [SerializeField, Range(0.0f, 1.0f)] private float _lerpStrength = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_owner == null) _owner = transform;
        if (_target == null)
        {
            GameObject target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) _target = target.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(_target.position, transform.position);

        if (_target != null && distance > _distance)
        {

            Vector3 position = Vector3.zero;

            // 座標を計算
            position = Vector3.Lerp(transform.position, _target.position + _offset,  _lerpStrength * Time.deltaTime);

            // 座標値を加算
            transform.position = position;
        }
        // ターゲットの方向を向く
        _owner.LookAt(_target.position);
    }
}