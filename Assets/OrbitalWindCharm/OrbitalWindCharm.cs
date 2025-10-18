//------------------------------------------------------------------------------
//     作成者   : Mariko Haruki
//     作成日   : 2025/10/16
//     概要     :
//     自分の周りをまわって自動的に周囲を冷やす風鈴を【一定時間】召喚するオブジェクト
//     プレイヤーが衝突すると風鈴パーティクルを生成し、一定時間プレイヤーの周りを周回する
//------------------------------------------------------------------------------
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class OrbitalWindCharm : MonoBehaviour
{
    [Header("衝突対象のタグ名")]
    [SerializeField] private string _tag = "Player";
    [SerializeField] private GameObject _player;     // 追従用に保持するプレイヤー

    [Header("生成設定")]
    [SerializeField] private GameObject _prefabWindCharm;   // 回るプレハブ（例：風鈴）
    [SerializeField] private int _count = 5;                // 生成個数
    [SerializeField] private float _radius = 2.0f;          // プレイヤーとの距離
    [SerializeField] private float _rotationSpeed = 30f;    // 回転速度（度/秒）
    [SerializeField] private float _lifeTime = 7.0f;        // 生存時間（秒）
    [SerializeField] private Vector3 _offsetPosition;       // オフセット座標


    private GameObject[] _orbitObjects;     // 生成した風鈴の配列
    private float _currentAngle = 0f;       // 現在の回転角度
    private float _timer = 0f;              // 生存時間管理

    private void Start()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        if (_player != null && _prefabWindCharm != null)
        {
            // このオブジェクト自体をプレイヤーの子階層に移動させる
            transform.SetParent(_player.transform, false);

            // プレハブ生成
            _orbitObjects = new GameObject[_count];
            for (int i = 0; i < _count; i++)
            {
                // 360度から要素の番号で割り、角度を計算する
                float angle = (360f / _count) * i * Mathf.Deg2Rad;
                // オフセットの座標を検出する
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _radius;
                // 生成する場所を計算
                Vector3 spawnPos = _player.transform.position + offset;

                // 風鈴を生成
                GameObject obj = Instantiate(_prefabWindCharm, spawnPos, Quaternion.identity);
                // プレイヤーの子階層にする
                obj.transform.SetParent(transform, true);
                _orbitObjects[i] = obj;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 更新処理
    private void Update()
    {
        if (_player == null || _orbitObjects == null) return;

        // タイマーを進める
        _timer += Time.deltaTime;
        if (_timer >= _lifeTime)
        {
            // 時間経過で全削除
            foreach (var obj in _orbitObjects)
            {
                if (obj != null) Destroy(obj);
            }
            Destroy(gameObject);
            return;
        }

        // 回転処理
        _currentAngle += _rotationSpeed * Time.deltaTime;

        for (int i = 0; i < _orbitObjects.Length; i++)
        {
            if (_orbitObjects[i] == null) continue;

            // 回転処理
            float angle = ((360f / _count) * i + _currentAngle) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _radius;
            _orbitObjects[i].transform.position = _player.transform.position + offset + _offsetPosition;
        }
    }
}
