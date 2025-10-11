using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using static UnityEditor.PlayerSettings;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RadialMenu : MonoBehaviour
{
    // カーソル情報（画像と色）
    [Serializable]
    public class CursorInformation
    {
        public Sprite _sourceImage;
        public Color _color = Color.white;
    }

    [Header("カメラ")]
    [SerializeField] private Camera _camera;

    [Header("ラジアルメニューのパラメータ")]
    [SerializeField] private bool _isShowCanvas = false;
    [SerializeField] private bool[] _hasReacheds; // 各要素が到達済みか管理
    [SerializeField] private RectTransform _centerRect = new RectTransform();
    [SerializeField, Tooltip("カーソルが移動できる半径")] private float _radiusCursorMoveRange = 100.0f;
    [SerializeField, Tooltip("カーソルが移動する速度")] private float _radiusCursorMoveForce = 10.0f;
    [SerializeField, Tooltip("カーソルでコマンドを選択できるギリギリのライン")] private float _radiusCursorActionRange = 0.3f;
    Canvas _canvas;
    RectTransform _canvasRect;
    Transform _tf;

    // ラジアルメニューに並ぶコマンドリスト
    [SerializeField] private List<RadialMenuCommandBase> _radialMenuList;

    [Header("カーソル")]
    [SerializeField, Tooltip("動かすカーソル")] private Image _cursorCurrent;
    [SerializeField, Tooltip("デフォルト状態")] private CursorInformation _cursorDefault;
    [SerializeField, Tooltip("重なっている状態")] private CursorInformation _cursorHovered;
    [SerializeField, Tooltip("押している状態")] private CursorInformation _cursorPressed;
    [SerializeField, Tooltip("離している状態")] private CursorInformation _cursorReleased;

    [Header("オンオフで表示非表示にするオブジェクト")]
    [SerializeField] private List<GameObject> _listSwitch;

    private void Awake()
    {
        // カメラが未設定ならメインカメラを取得
        if (_camera == null) _camera = Camera.main;

        _tf = transform;

        // Canvas取得
        _canvas = GetComponent<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();

        // コマンド数に応じて到達フラグ配列を初期化
        _hasReacheds = new bool[_radialMenuList.Count];

        // 初期状態でキャンバス非表示
        InactiveCanvas();
    }

    private void Update()
    {
        // キャンバスの表示切り替え
        SwitchCanvas();

        if (_isShowCanvas)
        {
            // カーソル移動
            MoveCursor();

            // まだ全て到達していない場合のみコマンドを移動
            if (!GetAllHasReacheds(true))
            {
                MoveCommands();
            }

            // コマンドのアクションを実行
            CheckCursorHover();
        }
    }

    // キャンバスのオンオフ、カーソル形状の切り替え
    private void SwitchCanvas()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ActiveCanvas();
        if (Input.GetKeyUp(KeyCode.Tab)) InactiveCanvas();

        if (Input.GetKeyDown(KeyCode.W)) SwitchCursor(_cursorDefault);
        if (Input.GetKeyDown(KeyCode.A)) SwitchCursor(_cursorHovered);
        if (Input.GetKeyDown(KeyCode.S)) SwitchCursor(_cursorPressed);
        if (Input.GetKeyDown(KeyCode.D)) SwitchCursor(_cursorReleased);
    }

    // マウスの位置にカーソルを移動
    private void MoveCursor()
    {
        // Canvasに設定されたカメラ優先、なければメインカメラ
        Camera cam = _canvas.worldCamera != null ? _canvas.worldCamera : Camera.main;

        // マウス座標をCanvasローカル座標に変換
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect,
            Input.mousePosition,
            cam,
            out localPos
        );

        // 中心を基準に半径制限
        Vector2 offset = localPos - _centerRect.anchoredPosition;
        if (offset.magnitude > _radiusCursorMoveRange)
        {
            offset = offset.normalized * _radiusCursorMoveRange;
        }

        // カーソル座標を設定
        _cursorCurrent.rectTransform.anchoredPosition = _centerRect.anchoredPosition + offset;
    }

    private Vector2 GetCursorDirection()
    {
        return Vector2.zero;
    }

    // 各コマンドを円形に移動
    private void MoveCommands()
    {
        for (int i = 0; i < _radialMenuList.Count; i++)
        {
            if (_hasReacheds[i]) continue; // 到達済みならスキップ

            // 360度を均等に分割し、1個目を上にする
            float angle = (360f / _radialMenuList.Count) * i + 90.0f;
            float rad = angle * Mathf.Deg2Rad;

            // 中心からの目標座標
            Vector2 targetPos = _centerRect.anchoredPosition + new Vector2(
                Mathf.Cos(rad) * _radiusCursorMoveRange,
                Mathf.Sin(rad) * _radiusCursorMoveRange
            );


            // 移動
            ReachTarget(_radialMenuList[i]._image.rectTransform, targetPos, i);
        }
    }

    // 全コマンドの座標を指定位置に設定
    private void SetRectCommands(Vector2 pos)
    {
        foreach (var command in _radialMenuList)
        {
            command._image.rectTransform.anchoredPosition = pos;
        }
    }

    // コマンドのアクション実行
    private void ActionCommand(RadialMenuCommandBase command)
    {
        command.Move();
    }

    // キャンバス表示
    private void ActiveCanvas()
    {
        _hasReacheds = new bool[_radialMenuList.Count]; // フラグ初期化
        SwitchActiveGameObjects(_listSwitch, true);
        SetRectCommands(Vector2.zero); // コマンドを中央に
        SetAllHasReacheds(false);      // 到達フラグリセット
        _isShowCanvas = true;
    }

    // キャンバス非表示
    private void InactiveCanvas()
    {
        CheckActionCommand();
        SwitchActiveGameObjects(_listSwitch, false);
        _isShowCanvas = false;
    }

    // 全ての到達フラグを指定値に設定
    private void SetAllHasReacheds(bool key)
    {
        for (int i = 0; i < _hasReacheds.Length; i++)
        {
            _hasReacheds[i] = key;
        }
    }

    // 全てのフラグが指定値か判定
    private bool GetAllHasReacheds(bool key)
    {
        for (int i = 0; i < _hasReacheds.Length; i++)
        {
            if (_hasReacheds[i] != key) return false;
        }
        return true;
    }

    // 指定位置まで移動させる
    private void ReachTarget(RectTransform rect, Vector2 targetPosition, int index)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPosition, _radiusCursorMoveForce * Time.deltaTime);

        // 目標に到達したかチェック
        if (Vector2.Distance(rect.anchoredPosition, targetPosition) <= _radiusCursorActionRange)
        {
            rect.anchoredPosition = targetPosition;
            _hasReacheds[index] = true; // 到達フラグ
        }
    }

    // オブジェクトのオンオフ切り替え
    private void SwitchActiveGameObjects(List<GameObject> list, bool key)
    {
        foreach (GameObject go in list)
        {
            go.SetActive(key);
        }
    }

    // カーソルの画像と色を変更
    private void SwitchCursor(CursorInformation cursorInformation)
    {
        _cursorCurrent.sprite = cursorInformation._sourceImage;
        _cursorCurrent.color = cursorInformation._color;
    }

    // Canvas毎にマウス座標を取得
    public static Vector2 GetMousePositionOnCanvas(Canvas canvas, Camera camera = null)
    {
        if (canvas == null)
        {
            Debug.LogWarning("Canvas が指定されていません。");
            return Vector2.zero;
        }

        if (camera == null)
        {
            camera = canvas.worldCamera;
            if (camera == null)
                camera = Camera.main;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 mousePos = Input.mousePosition;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePos,
            camera,
            out localPoint
        );

        return localPoint;
    }

    private void CheckCursorHover(float scaleFactor = 1.2f)
    {
        // カーソルのRectTransform取得
        RectTransform cursorRect = _cursorCurrent.rectTransform;

        for (int i = 0; i < _radialMenuList.Count; i++)
        {
            RectTransform commandRect = _radialMenuList[i]._image.rectTransform;

            // 接触判定（RectTransform同士の矩形判定）
            if (RectTransformOverlap(cursorRect, commandRect))
            {
                // 接触していたら少し大きくする
                commandRect.localScale = Vector3.one * scaleFactor;
            }
            else
            {
                // 接触していなければ元の大きさに戻す
                commandRect.localScale = Vector3.one;
            }
        }
    }

    /// <summary>
    /// 衝突判定
    /// </summary>
    private bool RectTransformOverlap(RectTransform rectA, RectTransform rectB)
    {
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];
        rectA.GetWorldCorners(cornersA);
        rectB.GetWorldCorners(cornersB);

        Rect a = new Rect(cornersA[0], cornersA[2] - cornersA[0]);
        Rect b = new Rect(cornersB[0], cornersB[2] - cornersB[0]);

        return a.Overlaps(b);
    }

    private void CheckActionCommand()
    {
        // カーソルのRectTransform
        RectTransform cursorRect = _cursorCurrent.rectTransform;

        for (int i = 0; i < _radialMenuList.Count; i++)
        {
            RectTransform commandRect = _radialMenuList[i]._image.rectTransform;

            // 接触していればMoveを実行
            if (RectTransformOverlap(cursorRect, commandRect) && _hasReacheds[i])
            {
                ActionCommand(_radialMenuList[i]);
            }
        }
    }
}