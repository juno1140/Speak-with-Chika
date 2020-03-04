using UnityEngine;

public class RuntimePermission : MonoBehaviour
{
    private string permission = "android.permission.RECORD_AUDIO"; // 確認するパーミッションを指定
    private bool permissionRequested = false;

    void Start()
    {
        if (!RuntimePermissionHelper.HasPermission(permission))
        {
            // パーミッションをリクエスト
            RuntimePermissionHelper.RequestPermission(new string[] { permission });
            permissionRequested = true;
        }
    }

    // パーミッションダイアログから戻ってきたときなどに呼ばれる
    void OnApplicationPause(bool pauseStatus)
    {
        // ポーズからの復帰時かつパーミッションリクエストの直後の場合
        if (!pauseStatus && permissionRequested)
        {
            // パーミッションを持っているかどうか
            if (RuntimePermissionHelper.HasPermission(permission))
            {
                Debug.Log("パーミッションリクエスト成功!");
                permissionRequested = false;
            }
        }
    }
}
