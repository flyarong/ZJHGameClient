using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelfManager_Stand : BaseManager_Stand
{
    public AudioClip clip_Start;
    public AudioClip clip_GiveUp;

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

    private void Awake()
    {
        EventCenter.AddListener<int>(EventDefine.UpdateCoinCount, UpdateCoinCount);
        
        Init();
    }

    private void FixedUpdate()
    {
        if (tog_2.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.gray;
            tog_5.GetComponent<Image>().color = Color.white;
            tog_10.GetComponent<Image>().color = Color.white;
        }else if (tog_5.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.white;
            tog_5.GetComponent<Image>().color = Color.gray;
            tog_10.GetComponent<Image>().color = Color.white;
        }else if (tog_10.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.white;
            tog_5.GetComponent<Image>().color = Color.white;
            tog_10.GetComponent<Image>().color = Color.gray;
        }

        if (m_IsStartStakes)
        {
            if (m_ZJHManager.IsSelfWin())
            {
                m_ZJHManager.SelfWin();
                m_IsStartStakes = false;
                return;
            }

            if (m_Time <= 0)
            {
                //倒计时结束
                //默认当作跟注处理  TODO
                OnFollowButtonClick();
                m_Time = 60;
            }
            m_Timer += Time.deltaTime;
            if (m_Timer >= 1)
            {
                m_Timer = 0;
                m_Time--;
                txt_CountDown.text = m_Time.ToString();
            }
        }
    }

    private void OnDeatory()
    {
        EventCenter.RemoveListener<int>(EventDefine.UpdateCoinCount, UpdateCoinCount);
    }

    private void Init()
    {
        //牌的间隔
        m_CardPointX = -70f;
        m_AudioSource = GetComponent<AudioSource>();
        m_StakesCountHint = transform.Find("StakesCountHint").GetComponent<StakesCountHint>();
        m_ZJHManager = GetComponentInParent<ZJHManager_Stand>();
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
    /// 与左边玩家比牌点击
    /// </summary>
    private void OnCompareLeftButtonClick()
    {
        m_ZJHManager.SelfComparedLeft();
        SetBottomButtonInteractable(false);
        go_CompareBtns.SetActive(false);
    }

    /// <summary>
    /// 与右边玩家比牌点击
    /// </summary>
    private void OnCompareRightButtonClick()
    {
        m_ZJHManager.SelfCompareRight();
        SetBottomButtonInteractable(false);
        go_CompareBtns.SetActive(false);
    }

    /// <summary>
    /// 比牌点击按钮
    /// </summary>
    private void OnCompareButtonClick()
    {
        go_CompareBtns.SetActive(true);
        if (m_ZJHManager.LeftIsGiveUp)
        {
            btn_CompareLeft.gameObject.SetActive(true);
        }
        else
        {
            btn_CompareLeft.gameObject.SetActive(false);
        }

        if (m_ZJHManager.RightIsGiveUp)
        {
            btn_CompareRight.gameObject.SetActive(true);
        }
        else
        {
            btn_CompareRight.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 加注按钮点击
    /// </summary>
    private void OnAddButtonClick()
    {
        if (tog_2.isOn)
        {
            StakesAfter(m_ZJHManager.Stakes(m_ZJHManager.Stakes(0) * 1), "不看");
        }
        else if (tog_5.isOn)
        {
            StakesAfter(m_ZJHManager.Stakes(m_ZJHManager.Stakes(0) * 4), "不看");
        }
        else if (tog_10.isOn)
        {
            StakesAfter(m_ZJHManager.Stakes(m_ZJHManager.Stakes(0) * 9), "不看");
        }
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        SetBottomButtonInteractable(false);
        m_ZJHManager.SetNextPlayerStakes();
    }

    /// <summary>
    /// 弃牌按钮点击
    /// </summary>
    private void OnGiveUpCardButtonClick()
    {
        m_AudioSource.clip = clip_GiveUp;
        m_AudioSource.Play();
        m_IsStartStakes = false;
        go_BottomButton.SetActive(false);
        go_CountDown.SetActive(false);
        m_IsGiveUpCard = true;
        txt_GiveUp.SetActive(true);

        foreach (var item in go_SpawnCardList)
        {
            Destroy(item);
        }

        m_ZJHManager.SetNextPlayerStakes();
    }

    /// <summary>
    /// 跟注点击
    /// </summary>
    private void OnFollowButtonClick()
    {
        int stakes = m_ZJHManager.Stakes(0);
        m_ZJHManager.SetNextPlayerStakes();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        SetBottomButtonInteractable(false);
        StakesAfter(stakes,"不看");
    }

    /// <summary>
    /// 下注之后
    /// </summary>
    /// <param name="count"></param>
    /// <param name="str"></param>
    protected override void StakesAfter(int count, string str)
    {
        base.StakesAfter(count, str);

        if (NetMsgCenter.Instance != null)
        {
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, -count);
        }
    }

    /// <summary>
    /// 看牌按钮点击
    /// </summary>
    private void OnLookCardButtonClick()
    {
        btn_LookCard.interactable=false;
        for (int i = 0; i < m_CardList.Count; i++)
        {
            string cardName = "card_" + m_CardList[i].Color.ToString() +"_"+ m_CardList[i].Weight.ToString();
            go_SpawnCardList[i].GetComponent<Image>().sprite = ResourceManager.LoadCardSprite(cardName);
        }
    }

    private void OnReadyButtonClick()
    {
        m_AudioSource.clip = clip_Start;
        m_AudioSource.Play();
        //更新总下注信息显示
        m_StakesSum += Models.GameModel.BottomStakes;
        txt_StakesSum.text = m_StakesSum.ToString();
        if (NetMsgCenter.Instance != null)
        {
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, -Models.GameModel.BottomStakes);
        }
        
        btn_Ready.gameObject.SetActive(false);
        m_ZJHManager.ChooseBanker();

    }

    /// <summary>
    /// 设置底部按钮是否可以点击
    /// </summary>
    /// <param name="value"></param>
    private void SetBottomButtonInteractable(bool value)
    {
        //btn_LookCard.interactable = value;
        btn_FollowStakes.interactable = value;
        btn_AddStakes.interactable = value;
        btn_CompareCard.interactable = value;
        btn_GiveUp.interactable = value;
        tog_2.interactable = value;
        tog_5.interactable = value;
        tog_10.interactable = value;
    }

    private void UpdateCoinCount(int value)
    {
        txt_CoinCount.text = value.ToString();
    }

   

    /// <summary>
    /// 发牌结束
    /// </summary>
    public void DealCardFinished()
    {
        go_BottomButton.SetActive(true);
        SetBottomButtonInteractable(false);

        SortCards();
        GetCardType();
        print("自身玩家牌型："+m_CardType);
    }

    /// <summary>
    /// 开始下注
    /// </summary>
    public override void StartStakes()
    {
        base.StartStakes();
        SetBottomButtonInteractable(true);
    }

    public override void DealCard(Card card, float duration, Vector3 initPos)
    {
        base.DealCard(card, duration, initPos);

        GameObject go = Instantiate(go_CardPre, cardPoints);
        go.GetComponent<RectTransform>().localPosition = initPos;
        go.GetComponent<RectTransform>().DOLocalMove(new Vector3(m_CardPointX, 0, 0), duration);
        go_SpawnCardList.Add(go);
        m_CardPointX += 70;
    }

    public override void Lose()
    {
        OnGiveUpCardButtonClick();
    }

    public override void Win()
    {
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        m_ZJHManager.m_CurrentStakesIndex = 0;
        m_ZJHManager.SetNextPlayerStakes();
    }
}
