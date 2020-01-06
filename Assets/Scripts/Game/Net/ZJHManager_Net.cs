using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Code;
using UnityEngine.SceneManagement;

public class ZJHManager_Net : MonoBehaviour {
    private Text txt_BottomStakes;
    private Text txt_TopStakes;
    private Button btn_Back;

    private void Awake()
    {
        if (NetMsgCenter.Instance!=null)
        {
            NetMsgCenter.Instance.SendMsg(OpCode.Match, MatchCode.Enter_CREQ, (int)Models.GameModel.RoomType);
        }
        Init();
    }

    private void Init()
    {
        txt_BottomStakes = transform.Find("Main/txt_BottomStakes").GetComponent<Text>();
        txt_TopStakes = transform.Find("Main/txt_TopStakes").GetComponent<Text>();
        btn_Back = transform.Find("Main/btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Main");
            NetMsgCenter.Instance.SendMsg(OpCode.Match, MatchCode.Leave_CREQ, (int)Models.GameModel.RoomType);
        });

        txt_BottomStakes.text = Models.GameModel.BottomStakes.ToString();
        txt_TopStakes.text = Models.GameModel.TopStakes.ToString();
    }
}
