using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class AndroidSpeechRecognizer : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Button _button;
    [SerializeField] private Text debug;
    private const string ANDROID_NATIVE_PLUGIN_CLASS = "com.junotaku.speechrecognizer.NativeSpeechRecognizer";
    AndroidJavaClass nativeRecognizer;
    AndroidJavaClass unityPlayer;
    AndroidJavaObject context;
    private bool enabledFlg = false;

    private void Start()
    {
#if UNITY_ANDROID
        nativeRecognizer = new AndroidJavaClass(ANDROID_NATIVE_PLUGIN_CLASS);
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
#endif
        // ボタンが押されたらスピーカーを開始する
        _button.onClick.AddListener(() =>
        {
            enabledFlg = true;
            _button.gameObject.SetActive(false);
            StartCoroutine("StartRecognizer");
            _text.text = "";
        });
        _text.text = "私とお話しようよ！";
    }

    //private void Update()
    //{
    //    if (enabledFlg)
    //    {
    //        StartCoroutine("StartRecognizer");
    //    }
    //}

    IEnumerator StartRecognizer()
    {
#if UNITY_ANDROID
        while (true)
        {
            context.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                nativeRecognizer.CallStatic(
                    "StartRecognizer",
                    context,
                    gameObject.name,
                    "CallbackMethod"
                );
            }));
            yield return new WaitForSeconds(5);
        }
#endif
    }

    private void CallbackMethod(string message)
    {
        debug.text = message;
        string[] messages = message.Split('\n');
        if (messages[0] == "onResults")
        {
            string msg = "";
            for (int i = 1; i < messages.Length; i++)
            {
                msg += messages[i];
            }
            ChangeMessage(msg);
        }
    }

    private void ChangeMessage(string msg)
    {
        int num;

        if (msg.Contains("輝きたい"))
        {
            num = 9;
        }
        else if (msg.Contains("自己紹介"))
        {
            num = 8;
        }
        else if (msg.Contains("Google"))
        {
            num = 7;
        }
        else if (msg.Contains("クローバ"))
        {
            num = 6;
        }
        else if (msg.Contains("アレクサ"))
        {
            num = 5;
        }
        else if(msg.Contains("君の心は"))
        {
            num = 4;
        }
        else if (msg.Contains("やめる"))
        {
            num = 3;
        }
        else if (msg.Contains("元気"))
        {
            num = 2;
        }
        else if (msg.Contains("ちかちゃん"))
        {
            num = 1;
        }
        else
        {
            num = 0;
        }

        switch (num)
        {
            case 1:
                _text.text = "ん？どうしたの？";
                break;
            case 2:
                _text.text = "うん、元気！";
                break;
            case 3:
                _text.text = "やめない！";
                break;
            case 4:
                _text.text = "輝いてるかい？";
                break;
            case 5:
                _text.text = "アレクサでもないーー！";
                break;
            case 6:
                _text.text = "クローバでもない！";
                break;
            case 7:
                _text.text = "千歌はグーグルじゃないよ！";
                break;
            case 8:
                _text.text = "浦の星女学院２年、高海千歌です！";
                break;
            case 9:
                _text.text = "全身全霊最後の最後まで、みんなで輝こう！";
                break;
            default:
                _text.text = "えっ、なに？何か言った？";
                break;
        }
        
    }
}
