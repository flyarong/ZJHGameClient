using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

enum GameType
{
    Net,
    StandAlone
}
public class RoomChoosePanel : MonoBehaviour {

    private Button btn_EnterRoom1;
    private Button btn_EnterRoom2;
    private Button btn_EnterRoom3;
    private Button btn_Close;
    private GameType m_GameType;

    private void Awake()
    {
        EventCenter.AddListener<GameType>(EventDefine.ShowRoomChoosePanel, Show);
        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameType>(EventDefine.ShowRoomChoosePanel, Show);
    }

    private void Init()
    {
        btn_EnterRoom1 = transform.Find("bg/Image/Rooms/Room_1/Button").GetComponent<Button>();
        btn_EnterRoom2 = transform.Find("bg/Image/Rooms/Room_2/Button").GetComponent<Button>();
        btn_EnterRoom3 = transform.Find("bg/Image/Rooms/Room_3/Button").GetComponent<Button>();

        btn_EnterRoom1.onClick.AddListener(delegate { EnterRoom(10, 100); });
        btn_EnterRoom2.onClick.AddListener(delegate { EnterRoom(20, 200); });
        btn_EnterRoom3.onClick.AddListener(delegate { EnterRoom(50, 500); });



        btn_Close = transform.Find("bg/Image/btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(() =>
        {
            OnCloseButtonClick();
        });
    }



    private void OnCloseButtonClick()
    {
        transform.DOScale(Vector3.zero, 0.3f);
    }

    private void Show(GameType gameType)
    {
        m_GameType = gameType;
        transform.DOScale(Vector3.one, 0.3f);
    }

    private void EnterRoom(int bottomStakes,int topStakes)
    {
        Models.GameModel.BottomStakes = bottomStakes;
        Models.GameModel.TopStakes = topStakes;

        switch (m_GameType)
        {
            case GameType.Net:
                //进入联网游戏场景
                break;
            case GameType.StandAlone:
                //进入单机游戏场景
                SceneManager.LoadScene("StandAlone");
                break;
            default:
                break;
        }
    }
}
