using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState
{
    None,
    In,
    Out
}

public class FadeMachine : MonoBehaviour
{
    [SerializeField] private FadeState _state;
    public void SetState(FadeState state) {  _state = state; }

    [Header("フェード情報")]
    [SerializeField] private Image _image;
    [SerializeField, Range(0.0f, 5.0f)] private float _fadeSpeed = 0.5f;
    [SerializeField] private float _inBeginAlpha = 1.0f;
    [SerializeField] private float _outBeginAlpha = 0.0f;
    [SerializeField] private float _axisAlpha = 0.0f;

    private void Update()
    {
        if (_image != null)
        {
            Color alpha = _image.color;
            alpha.a = Mathf.Lerp(_axisAlpha, _image.color.a, Time.deltaTime * _fadeSpeed);
            _image.color = alpha;
        }
    }

    public void FadeIn()
    {
        if (_image == null) return;
        if (_state == FadeState.In) return;

        Color color = _image.color;
        if (_state == FadeState.None) color.a = _inBeginAlpha;
        _image.color = color;

        _state = FadeState.In;
    }

    public void FadeOut()
    {
        if (_image == null) return;
        if (_state == FadeState.Out) return;

        _state = FadeState.Out;
    }
}
