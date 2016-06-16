using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;

public class MenuPanel : MonoBehaviour
{

    public static MenuPanel Instance;
    public GameObject scorePanel;
    public GameObject btnShare;
    public GameObject btnStart;
    public GameObject btnScore;
    public GameObject btnHelp;
    private ShareSDK ssdk;
    void Awake()
    {
        Instance = this;
        ssdk = this.GetComponent<ShareSDK>();
        ssdk.shareHandler = ShareResultHandler;
    }
    void Start()
    {
        UIEventListener.Get(btnStart).onClick = go => OnBtnStart();
        UIEventListener.Get(btnScore).onClick = go => OnBtnScore();
        UIEventListener.Get(btnHelp).onClick = go => OnBtnHelp();
        UIEventListener.Get(btnShare).onClick = go => OnBtnShare();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MessageBox.Instance.ShowInfo("是否退出游戏?");
        }
    }
    /// <summary>
    /// 开始按钮
    /// </summary>
    void OnBtnStart()
    {
        Debug.Log("开始按钮");
        Application.LoadLevel(1);
    }
    /// <summary>
    /// 统计按钮
    /// </summary>
    void OnBtnScore()
    {
        Debug.Log("统计按钮");
    }
    /// <summary>
    /// 帮助按钮
    /// </summary>
    void OnBtnHelp()
    {
        this.gameObject.SetActive(false);
        scorePanel.SetActive(true);
        Debug.Log("帮助按钮");
    }
    /// <summary>
    /// 分享按钮
    /// </summary>
    void OnBtnShare()
    {
        ShareContent ss = new ShareContent();
        ss.SetText("你好欢迎来到2048");
        ss.SetTitle("2048");
        ss.SetUrl("https://www.baidu.com/");
        ss.SetShareType(ContentType.App);
        ss.SetImageUrl("https://f1.webshare.mob.com/code/demo/img/1.jpg");
        ssdk.ShareContent(PlatformType.WeChat, ss);
        Debug.Log("分享按钮");
    }
    /// <summary>
    /// 分享的回掉函数
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="result"></param>
    void ShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("share result :");
            print(MiniJSON.jsonEncode(result));
            MessageBox.Instance.ShowInfo("分享成功");
        }
        else if (state == ResponseState.Fail)
        {
            print("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
            MessageBox.Instance.ShowInfo("分享失败");
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
            MessageBox.Instance.ShowInfo("分享取消");
        }
    }
}
