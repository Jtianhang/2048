using UnityEngine;
using System.Collections;

public class ScorePanel : MonoBehaviour
{
    public MenuPanel menuPanel;
    public GameObject btnClose;
    void Start()
    {
        UIEventListener.Get(btnClose).onClick = go => OnBtnClose();
    }
    /// <summary>
    /// 关闭按钮
    /// </summary>
    void OnBtnClose()
    {
        this.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);
    }
}
