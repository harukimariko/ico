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
    // フェードステート
    [SerializeField] private FadeState _state;
    public void SetState(FadeState state) { _state = state; }

    [Header("フェード制御")]
    [SerializeField] private Image _image;

    [Header("フェードステータス")]
    [SerializeField, Range(0.0f, 5.0f)] private float _fadeSpeed = 0.5f;
    [SerializeField] private float _inBeginAlpha = 1.0f;
    [SerializeField] private float _outBeginAlpha = 0.0f;
    [SerializeField] private float _inEndAlpha = 0.0f;
    [SerializeField] private float _outEndAlpha = 1.0f;
    [SerializeField] private float _axisAlpha = 0.0f;

    private void Update()
    {
        // カラー軸を用いたカラー変更処理
        UpdateFadeColor();

        if (Input.GetKeyUp(KeyCode.T)) FadeIn();
        if (Input.GetKeyUp(KeyCode.Y)) FadeOut();
    }

    /// <summary>
    /// フェードイン処理
    /// </summary>
    public void FadeIn()
    {
        // ねずみがえし
        if (_image == null) return;
        if (_state == FadeState.In) return;

        Color color = _image.color;
        if (_state == FadeState.None) color.a = _inBeginAlpha;
        _axisAlpha = _inEndAlpha;
        _image.color = color;

        _state = FadeState.In;
    }

    /// <summary>
    /// フェードアウト関数
    /// 外部からこれを呼べばフェードアウトできるようにする
    /// </summary>
    public void FadeOut()
    {
        if (_image == null) return;
        if (_state == FadeState.Out) return;

        Color color = _image.color;
        if (_state == FadeState.None) color.a = _outBeginAlpha;
        _axisAlpha = _outEndAlpha;
        _image.color = color;

        _state = FadeState.Out;
    }

    /// <summary>
    /// アルファ値の更新関数
    /// </summary>
    private void UpdateFadeColor()
    {
        if (_image != null)
        {
            Color alpha = _image.color;
            alpha.a = Mathf.Lerp(_image.color.a, _axisAlpha, Time.deltaTime * _fadeSpeed);
            _image.color = alpha;
        }
    }
}