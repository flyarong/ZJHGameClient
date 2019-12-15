using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfManager_Net : MonoBehaviour {

    public GameObject go_CardPre;

    protected Image img_Banker;
    protected Transform cardPoints;
    protected GameObject go_CountDown;
    protected Text txt_CountDown;
    protected Text txt_StakesSum;
    protected Image img_HeadIcon;

    protected StakesCountHint m_StakesCountHint;

    private GameObject go_BottomButton;
    private Text txt_UserName;
    private Text txt_CoinCount;

    private Button btn_Ready;
    private GameObject txt_GiveUp;

    private Button btn_LookCard;
    private Button btn_FollowStakes;
    private Button btn_AddStakes;
    private Button btn_CompareCard;
    private Button btn_GiveUp;
    private Toggle tog_2;
    private Toggle tog_5;
    private Toggle tog_10;
    private GameObject go_CompareBtns;
    private Button btn_CompareLeft;
    private Button btn_CompareRight;
    private AudioSource m_AudioSource;

    //牌的间隔
    protected float m_CardPointX = -70f;

    private void AWake()
    {
        Init();
    }

    private void Init()
    {
        //牌的间隔
        m_CardPointX = -70f;
        m_StakesCountHint = transform.Find("StakesCountHint").GetComponent<StakesCountHint>();
        go_BottomButton = transform.Find("BottomButton").gameObject;
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        txt_UserName = transform.Find("img_HeadIcon/txt_UserName").GetComponent<Text>();
        txt_CoinCount = transform.Find("Coin/txt_CoinCount").GetComponent<Text>();
        img_Banker = transform.Find("img_HeadIcon/img_Banker").GetComponent<Image>();
        txt_StakesSum = transform.Find("StakesSum/txt_StakesSum").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        btn_Ready = transform.Find("btn_Ready").GetComponent<Button>();
        txt_GiveUp = transform.Find("txt_GiveUp").gameObject;
        cardPoints = transform.Find("CardPoints");

        btn_LookCard = transform.Find("BottomButton/btn_LookCard").GetComponent<Button>();
        btn_FollowStakes = transform.Find("BottomButton/btn_FollowStakes").GetComponent<Button>();
        btn_AddStakes = transform.Find("BottomButton/btn_AddStakes").GetComponent<Button>();
        btn_CompareCard = transform.Find("BottomButton/btn_CompareCard").GetComponent<Button>();
        btn_GiveUp = transform.Find("BottomButton/btn_GiveUp").GetComponent<Button>();
        tog_2 = transform.Find("BottomButton/tog_2").GetComponent<Toggle>();
        tog_5 = transform.Find("BottomButton/tog_5").GetComponent<Toggle>();
        tog_10 = transform.Find("BottomButton/tog_10").GetComponent<Toggle>();
        go_CompareBtns = transform.Find("CompareBtns").gameObject;
        btn_CompareLeft = go_CompareBtns.transform.Find("btn_CompareLeft").GetComponent<Button>();
        btn_CompareRight = go_CompareBtns.transform.Find("btn_CompareRight").GetComponent<Button>();

        btn_LookCard.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_FollowStakes.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_AddStakes.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_CompareCard.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_GiveUp.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_2.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_5.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_10.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        btn_LookCard.onClick.AddListener(OnLookCardButtonClick);
        btn_FollowStakes.onClick.AddListener(OnFollowButtonClick);
        btn_GiveUp.onClick.AddListener(OnGiveUpCardButtonClick);
        btn_AddStakes.onClick.AddListener(OnAddButtonClick);
        btn_CompareCard.onClick.AddListener(OnCompareButtonClick);
        btn_CompareLeft.onClick.AddListener(OnCompareLeftButtonClick);
        btn_CompareRight.onClick.AddListener(OnCompareRightButtonClick);

        go_BottomButton.SetActive(false);
        img_Banker.gameObject.SetActive(false);
        txt_GiveUp.SetActive(false);
        go_CountDown.SetActive(false);
        go_CompareBtns.SetActive(false);
        btn_Ready.onClick.AddListener(() =>
        {
            OnReadyButtonClick();
        });

        txt_StakesSum.text = "0";
        if (Models.GameModel.userDto != null)
        {
            img_HeadIcon.sprite = ResourceManager.GetSprite(Models.GameModel.userDto.IconName);
            txt_UserName.text = Models.GameModel.userDto.UserName;
            txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
        }
    }

    /// <summary>
    /// 看牌
    /// </summary>
    private void OnLookCardButtonClick()
    {

    }

    /// <summary>
    /// 跟注
    /// </summary>
    private void OnFollowButtonClick()
    {

    }

    /// <summary>
    /// 棋牌
    /// </summary>
    private void OnGiveUpCardButtonClick()
    {

    }

    /// <summary>
    /// 加注
    /// </summary>
    private void OnAddButtonClick()
    {

    }

    /// <summary>
    /// 比牌
    /// </summary>
    private void OnCompareButtonClick()
    {

    }

    /// <summary>
    /// 左家比牌
    /// </summary>
    private void OnCompareLeftButtonClick()
    {

    }

    /// <summary>
    /// 右家比牌
    /// </summary>
    private void OnCompareRightButtonClick()
    {

    }

    /// <summary>
    /// 准备按钮
    /// </summary>
    private void OnReadyButtonClick()
    {

    }
    
}
