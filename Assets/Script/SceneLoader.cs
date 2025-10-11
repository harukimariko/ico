using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    private float _delayTime = 0f;

    /// <summary>
    /// �f�B���C���Ԃ�ݒ肷��
    /// </summary>
    public void SetDelay(float seconds)
    {
        _delayTime = seconds;
    }

    /// <summary>
    /// �w�肵���V�[�������[�h�i�f�B���C�t���j
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        if (_delayTime > 0f)
        {
            yield return new WaitForSeconds(_delayTime);
        }

        SceneManager.LoadScene(sceneName);
    }
}
