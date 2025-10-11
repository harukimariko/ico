using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum PlayState
{
    Ready,
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

    [Header("UI制御")]
    [SerializeField] private IrisShot _irisShot;
    [SerializeField] private FadeMachine _fadeMachine;

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
            Destroy(gameObject);
        }

        _instance.InitGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーンの中からScoreTextを探す
        GameObject textObj = GameObject.Find("ScoreText");
        InitGame();
    }

    public void InitGame()
    {
        // HPの初期化
        SetHp(0);

        // カーソルを表示する
        Cursor.visible = true;

        // カーソルを固定しない（画面外に動かせるようにする）
        Cursor.lockState = CursorLockMode.None;

        // アイリスイン
        _irisShot.IrisIn();
    }

    public void ChangeState(PlayState state)
    {
        _state = state;
    }

    public void SetHp(float hp)
    {
        _hp = hp;
        _hpText.text = _hp.ToString("F2");
    }

    private void SetActiveGameObjects(List<GameObject> list, bool key)
    {
        foreach(var obj in list)
        {
            obj.SetActive(key);
        }
    }

    public void GameClear()
    {

    }
}
