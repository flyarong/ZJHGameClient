using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class ComputerBaseManager_Stand : BaseManager_Stand {


    private GameObject txt_Ready;
    private GameObject txt_GiveUp;

    private float m_RandomWaitStakesTime = 0.0f;

    //是否有下注次数
    private bool m_IsHasStakesNum = false;

    //下注次数
    private int m_StakesNum = 0;

    protected bool m_IsComparing = false;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //牌的间隔
        m_CardPointX = -40f;
        m_ZJHManager = GetComponentInParent<ZJHManager_Stand>();
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        img_Banker = transform.Find("img_HeadIcon/img_Banker").GetComponent<Image>();
        txt_StakesSum = transform.Find("StakesSum/txt_StakesSum").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        txt_Ready = transform.Find("txt_Ready").gameObject;
        txt_GiveUp = transform.Find("txt_GiveUp").gameObject;
        cardPoints = transform.Find("CardPoints");
        m_StakesCountHint = transform.Find("StakesCountHint").GetComponent<StakesCountHint>();

        txt_GiveUp.SetActive(false);
        txt_StakesSum.text = "0";
        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        int ran = Random.Range(0, 19);
        string name = "headIcon_" + ran;
        img_HeadIcon.sprite = ResourceManager.GetSprite(name);
    }

    public override void Lose()
    {
        GiveUpCard();
    }

    public override void Win()
    {
        
    }

    /// <summary>
    /// 下注
    /// </summary>
    private void PutStakes()
    {
        if (m_IsHasStakesNum)
        {
            m_StakesNum--;
            if (m_StakesNum <= 0)
            {
                GetPutStakesNum();
                //比牌
                m_IsComparing = true;
                Compare();
                StakesAfter(m_ZJHManager.Stakes(Random.Range(4, 6)), "看看");
                return;
            }
            int stakes = m_ZJHManager.Stakes(Random.Range(3, 6));
            StakesAfter(stakes, "不看");
        }
        else if (m_CardType == CardType.Duizi)
        {
            int ran = Random.Range(0, 10);
            if (ran < 5)
            {
                StakesAfter(m_ZJHManager.Stakes(Random.Range(3, 6)), "不看");
            }
            else
            {
                //比牌
                m_IsComparing = true;
                Compare();
                StakesAfter(m_ZJHManager.Stakes(Random.Range(4, 6)), "看看");
            }
        }
        else if (m_CardType == CardType.Min)
        {
            int ran = Random.Range(0, 15);
            if (ran < 5)
            {
                StakesAfter(m_ZJHManager.Stakes(Random.Range(3, 6)), "不看");
            }
            else if (ran >= 5 && ran < 10)
            {
                //比牌
                m_IsComparing = true;
                Compare();
                StakesAfter(m_ZJHManager.Stakes(Random.Range(4, 6)), "看看");
            }
            else
            {
                //弃牌TODO
                GiveUpCard();
            }
        }
        else if (m_CardType == CardType.Baozi || m_CardType == CardType.Max || m_CardType == CardType.Jinhua)
        {
            StakesAfter(m_ZJHManager.Stakes(Random.Range(3, 6)), "不看");
        }
    }

    private void GiveUpCard()
    {
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        txt_GiveUp.SetActive(true);
        m_ZJHManager.SetNextPlayerStakes();
        m_IsGiveUpCard = true;
        foreach (var item in go_SpawnCardList)
        {
            Destroy(item);
        }
    }

    private void GetPutStakesNum()
    {
        if ((int)m_CardType >= 2 && (int)m_CardType <= 4)
        {
            m_IsHasStakesNum = true;
            m_StakesNum = (int)m_CardType;
        }
    }



    private void FixedUpdate()
    {

        if (m_IsStartStakes)
        {

            if (IsWin())
            {
                m_IsStartStakes = false;
                return;
            }

            if (m_Time <= 0)
            {
                //倒计时结束
                //默认当作跟注处理  TODO
                m_IsStartStakes = false;
                m_Time = 60;
            }

            //倒计时结束，下注
            if (m_RandomWaitStakesTime <= 0)
            {
                //开始下注
                PutStakes();
                m_IsStartStakes = false;
                if (m_IsComparing==false)
                {
                    go_CountDown.SetActive(false);
                    m_ZJHManager.SetNextPlayerStakes();
                }
                return;
            }

            m_Timer += Time.deltaTime;
            if (m_Timer >= 1)
            {
                m_RandomWaitStakesTime--;
                m_Timer = 0;
                m_Time--;
                txt_CountDown.text = m_Time.ToString();
            }
        }
    }

    public void StartChooseBanker()
    {
        //更新总下注信息显示
        m_StakesSum += Models.GameModel.BottomStakes;
        txt_StakesSum.text = m_StakesSum.ToString();
        txt_Ready.SetActive(false);
    }


    /// <summary>
    /// 发牌结束
    /// </summary>
    public void DealCardFinished()
    {
        SortCards();
        GetCardType();
        print("左边玩家牌型：" + m_CardType);
    }

    /// <summary>
    /// 开始下注
    /// </summary>
    public override void StartStakes()
    {
        base.StartStakes();
        m_RandomWaitStakesTime = Random.Range(3, 6);
    }

    public override void DealCard(Card card, float duration, Vector3 initPos)
    {
        base.DealCard(card, duration, initPos);

        GameObject go = Instantiate(go_CardPre, cardPoints);
        go.GetComponent<RectTransform>().localPosition = initPos;
        go.GetComponent<RectTransform>().DOLocalMove(new Vector3(m_CardPointX, 0, 0), duration);
        go.GetComponent<RectTransform>().DOScale(new Vector3(0.6f, 0.6f, 0.6f), duration);
        go_SpawnCardList.Add(go);
        m_CardPointX += 40;
    }

    public abstract void Compare();

    /// <summary>
    /// 判断是否胜利
    /// </summary>
    /// <returns></returns>
    public abstract bool IsWin();
}
