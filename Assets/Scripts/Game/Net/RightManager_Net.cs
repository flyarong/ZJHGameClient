using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightManager_Net : MonoBehaviour {
    public GameObject go_CardPre;

    protected Image img_Banker;
    protected Transform cardPoints;
    protected GameObject go_CountDown;
    protected Text txt_CountDown;
    protected Text txt_StakesSum;
    protected Image img_HeadIcon;

    protected StakesCountHint m_StakesCountHint;

    private Text txt_Hint;

    private Text txt_UserName;
    private GameObject go_Coin;
    private Text txt_CoinCout;
    private GameObject go_StakesSum;


    //牌的间隔
    protected float m_CardPointX = -40f;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.RefreshUI, RefreshUI);
        EventCenter.AddListener(EventDefine.StartGame, StartGame);
        Init();
    }

    private void OnDestroy()
    {

        EventCenter.RemoveListener(EventDefine.RefreshUI, RefreshUI);
        EventCenter.RemoveListener(EventDefine.StartGame, StartGame);
    }

    private void Init()
    {
        //牌的间隔
        m_CardPointX = -40f;
        go_StakesSum = transform.Find("StakesSum").gameObject;
        go_Coin = transform.Find("Coin").gameObject;
        txt_CoinCout = go_Coin.transform.Find("txt_CoinCount").GetComponent<Text>();
        txt_UserName = transform.Find("txt_UserName").GetComponent<Text>();
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        img_Banker = transform.Find("img_HeadIcon/img_Banker").GetComponent<Image>();
        txt_StakesSum = transform.Find("StakesSum/txt_StakesSum").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        txt_Hint = transform.Find("txt_Hint").GetComponent<Text>();
        cardPoints = transform.Find("CardPoints");
        m_StakesCountHint = transform.Find("StakesCountHint").GetComponent<StakesCountHint>();
        
        txt_StakesSum.text = "0";
        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        txt_Hint.gameObject.SetActive(false);
        txt_UserName.gameObject.SetActive(false);
        img_HeadIcon.gameObject.SetActive(false);
        go_StakesSum.SetActive(false);
        go_Coin.SetActive(false);
    }
    /// <summary>
    /// 当有新玩家进来时、有玩家离开时或自身玩家进来时调用这个方法，刷新一下界面
    /// </summary>
    private void RefreshUI()
    {
        MatchRoomDto room = Models.GameModel.matchRoomDto;

        if (room.RightPlayerId != -1)
        {
            UserDto dto = room.userIdUserDtoDic[room.RightPlayerId];
            img_HeadIcon.gameObject.SetActive(true);
            img_HeadIcon.sprite = ResourceManager.GetSprite(dto.IconName);
            go_Coin.SetActive(true);
            txt_CoinCout.text = dto.CoinCount.ToString();
            go_StakesSum.SetActive(true);
            txt_UserName.gameObject.SetActive(true);
            txt_UserName.text = dto.UserName;

            //右边玩家在准备中
            if (room.readyUserIdList.Contains(room.RightPlayerId))
            {
                txt_Hint.text = "已准备";
                txt_Hint.gameObject.SetActive(true);
            }
            else
            {
                txt_Hint.gameObject.SetActive(false);
            }
        }
        else
        {
            txt_Hint.gameObject.SetActive(false);
            img_HeadIcon.gameObject.SetActive(false);
            go_Coin.SetActive(false);
            go_StakesSum.SetActive(false);
            txt_UserName.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void StartGame()
    {
        txt_Hint.gameObject.SetActive(false);
    }
}
