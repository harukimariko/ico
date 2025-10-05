using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIParticle : MonoBehaviour
{
    [SerializeField] private int _value = 0;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    public ParticleSystem _healEffect;

    private void Awake()
    {
        _value = 0;
        _textMeshPro.text =_value.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            _value += 1;
            _textMeshPro.text = _value.ToString();
            PlayHealEffect();
        }
    }

    public void PlayHealEffect()
    {
        Debug.Log("çƒê∂Ç≥ÇÍÇƒÇÈÇ›ÇΩÇ¢ÇæÇÊ");
        _healEffect.Play();
    }
}
