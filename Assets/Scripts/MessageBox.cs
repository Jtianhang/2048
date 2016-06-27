using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour
{
    static MessageBox _instance;
    /// <summary>
    /// MessageBox动态单例
    /// </summary>
    public static MessageBox Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject prefab = (GameObject)Instantiate(Resources.Load("Prefabs/MessageBox"));
                prefab.transform.parent = GameObject.Find("UI Root").transform;
                prefab.transform.localPosition = Vector3.zero;
                prefab.transform.localScale = Vector3.zero;
                _instance = prefab.GetComponent<MessageBox>();
            }
            return _instance;
        }
    }
    public UILabel desLable;
    public GameObject btnYes;
    public GameObject btnCanel;
    public GameObject btnClose;
    public TweenScale tweenScale;
    void Start()
    {
        UIEventListener.Get(btnYes).onClick = go => OnBtnYes();
        UIEventListener.Get(btnCanel).onClick = go => OnBtnCanel();
        UIEventListener.Get(btnClose).onClick = go => OnBtnCanel();
    }
    public void ShowInfo(string strDes)
    {
        if (GamePanel.Instance != null)
        {
            PlayerPrefs.SetInt("BestScore", GamePanel.Instance.GetBestScore());
            PlayerPrefs.Save();
        }
        this.gameObject.SetActive(true);
        tweenScale.PlayForward();
        desLable.text = strDes;
    }

    void OnDestroy()
    {
        _instance = null;
    }
    /// <summary>
    /// 确认按钮
    /// </summary>
    void OnBtnYes()
    {
        if (Application.loadedLevel == 1)
        {
            Application.LoadLevel(0);
        }
        else if (Application.loadedLevel == 0)
        {
            Application.Quit();
        }

    }
    /// <summary>
    /// 取消按钮
    /// </summary>
    void OnBtnCanel()
    {
        tweenScale.AddOnFinished(Hold);
        tweenScale.PlayReverse();
    }
    void Hold()
    {
        this.gameObject.SetActive(false);
        EventDelegate del = new EventDelegate(Hold);
        tweenScale.RemoveOnFinished(del);
    }
}
