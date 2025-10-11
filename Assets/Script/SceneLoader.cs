using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    private float _delayTime = 0f;

    /// <summary>
    /// ディレイ時間を設定する
    /// </summary>
    public void SetDelay(float seconds)
    {
        _delayTime = seconds;
    }

    /// <summary>
    /// 指定したシーンをロード（ディレイ付き）
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
