using UnityEngine;

public class OrbitalWindCharmDrop : MonoBehaviour
{
    [Header("プレイヤー")]
    [SerializeField] private GameObject _player;
    [Header("生成する風鈴")]
    [SerializeField] private GameObject _prefabWindCharm;

    private void Start()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject== _player)
        {
            // プレハブを生成
            var prefab = Instantiate(_prefabWindCharm, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
