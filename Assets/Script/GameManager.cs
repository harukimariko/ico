using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum PlayState
{
    Play,
    Stop,
    Defeat,
    Clear
}

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    private PlayState _state;

    [Header("HP")]
    public float _hp { get; private set; }
    [SerializeField] private Text _hpText;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this);
        }

        _instance.InitGame();
    }

    public void InitGame()
    {
        SetHp(0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーンの中からScoreTextを探す
        GameObject textObj = GameObject.Find("ScoreText");
        if (textObj != null)
        {
            UpdateUI(); // 表示を更新
        }
    }

    public void ChangeState(PlayState state)
    {
        _state = state;
    }

    public void SetHp(float hp)
    {
        _hp = hp;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_hpText != null)
        {
            _hpText.text = _hp.ToString("F2");
        }
    }

    public void GameClear()
    {

    }
}
