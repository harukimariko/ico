using StarterAssets;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private ThirdPersonController _controller;

    private void Awake()
    {
        _panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // �p�l���\��
            _panel.SetActive(true);
        }
    }
}
