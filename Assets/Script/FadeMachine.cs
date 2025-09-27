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
    // �t�F�[�h�X�e�[�g
    [SerializeField] private FadeState _state;
    public void SetState(FadeState state) { _state = state; }

    [Header("�t�F�[�h����")]
    [SerializeField] private Image _image;

    [Header("�t�F�[�h�X�e�[�^�X")]
    [SerializeField, Range(0.0f, 5.0f)] private float _fadeSpeed = 0.5f;
    [SerializeField] private float _inBeginAlpha = 1.0f;
    [SerializeField] private float _outBeginAlpha = 0.0f;
    [SerializeField] private float _inEndAlpha = 0.0f;
    [SerializeField] private float _outEndAlpha = 1.0f;
    [SerializeField] private float _axisAlpha = 0.0f;

    private void Update()
    {
        // �J���[����p�����J���[�ύX����
        UpdateFadeColor();

        if (Input.GetKeyUp(KeyCode.T)) FadeIn();
        if (Input.GetKeyUp(KeyCode.Y)) FadeOut();
    }

    /// <summary>
    /// �t�F�[�h�C������
    /// </summary>
    public void FadeIn()
    {
        // �˂��݂�����
        if (_image == null) return;
        if (_state == FadeState.In) return;

        Color color = _image.color;
        if (_state == FadeState.None) color.a = _inBeginAlpha;
        _axisAlpha = _inEndAlpha;
        _image.color = color;

        _state = FadeState.In;
    }

    /// <summary>
    /// �t�F�[�h�A�E�g�֐�
    /// �O�����炱����Ăׂ΃t�F�[�h�A�E�g�ł���悤�ɂ���
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
    /// �A���t�@�l�̍X�V�֐�
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