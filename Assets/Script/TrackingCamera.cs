using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("ターゲット")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, -4f);

    [Header("カメラ設定")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float minPitch = -35f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private float defaultDistance = 4f;
    [SerializeField] private float smoothSpeed = 10f;

    [Header("当たり判定")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionBuffer = 0.2f; // 壁に近づく余裕

    private float yaw;
    private float pitch;
    private float currentDistance;
    private Vector3 currentPos;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        currentDistance = defaultDistance;
        currentPos = transform.position;
    }

    void LateUpdate()
    {
        if (!target) return;

        // --- マウス入力 ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // --- カメラ回転 ---
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // --- デフォルトの理想位置 ---
        Vector3 idealOffset = new Vector3(0, offset.y, -defaultDistance);
        Vector3 desiredPosition = target.position + rotation * idealOffset;

        // --- 壁との当たり判定（ズームイン） ---
        Vector3 targetCenter = target.position + Vector3.up * offset.y;
        if (Physics.Linecast(targetCenter, desiredPosition, out RaycastHit hit, collisionMask))
        {
            // 衝突距離 - バッファを現在の距離として使用
            currentDistance = Mathf.Clamp(Vector3.Distance(targetCenter, hit.point) - collisionBuffer, 0.5f, defaultDistance);
        }
        else
        {
            // 衝突していなければ徐々に元の距離に戻す
            currentDistance = Mathf.Lerp(currentDistance, defaultDistance, Time.deltaTime * smoothSpeed);
        }

        // --- カメラの最終位置 ---
        Vector3 finalOffset = new Vector3(0, offset.y, -currentDistance);
        desiredPosition = target.position + rotation * finalOffset;

        // --- スムーズ移動 ---
        currentPos = Vector3.Lerp(currentPos, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = currentPos;

        // --- カメラ方向 ---
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}
