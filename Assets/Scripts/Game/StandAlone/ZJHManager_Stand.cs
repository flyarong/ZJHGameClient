using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZJHManager_Stand : MonoBehaviour
{

    //发牌动画持续时间
    public float m_DealCardDurationTime = 0.2f;
    private Text txt_BottomStakes;
    private Text txt_TopStakes;
    private Button btn_Back;
    private LeftManager_Stand m_LeftManager;
    private RightManager_Stand m_RightManager;
    private SelfManager_Stand m_SelfManager;

    /// <summary>
    /// 当前发牌的游标
    /// </summary>
    private int m_CurrentSendCardIndex = 0;
    /// <summary>
    /// 当前下注的游标
    /// </summary>
    private int m_CurrentStakesIndex = 0;

    /// <summary>
    /// 牌库
    /// </summary>
    private List<Card> m_CardList = new List<Card>();

    /// <summary>
    /// 发牌的下标
    /// </summary>
    private int m_DealCardIndex = 0;

    //是否开始下注
    private bool m_IsStartStakes = false;

    //下一位玩家是否可以下注
    private bool m_IsNextPlayerCanStakes = true;

    public void SetNextPlayerStakes()
    {
        m_IsNextPlayerCanStakes = true;
    }

    //上一位玩家下注的数量
    private int m_LastPlayerStakesCount = 0;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_RightManager = transform.Find("Right").GetComponent<RightManager_Stand>();
        m_LeftManager = transform.Find("Left").GetComponent<LeftManager_Stand>();
        m_SelfManager = GetComponentInChildren<SelfManager_Stand>();
        txt_BottomStakes = transform.Find("Main/txt_BottomStakes").GetComponent<Text>();
        txt_TopStakes = transform.Find("Main/txt_TopStakes").GetComponent<Text>();
        btn_Back = transform.Find("Main/btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Main");
        });

        txt_BottomStakes.text = Models.GameModel.BottomStakes.ToString();
        txt_TopStakes.text = Models.GameModel.TopStakes.ToString();
        m_LastPlayerStakesCount = Models.GameModel.BottomStakes;
    }

    private void FixedUpdate()
    {
        if (m_IsStartStakes)
        {
            if (m_IsNextPlayerCanStakes)
            {
                //自身下注
                if (m_CurrentStakesIndex % 3 == 0)
                {
                    if (m_SelfManager.m_IsGiveUpCard == false)
                    {
                        m_SelfManager.StartStakes();
                        m_IsNextPlayerCanStakes = false;
                    }

                }
                //左边玩家下注
                if (m_CurrentStakesIndex % 3 == 1)
                {
                    if (m_LeftManager.m_IsGiveUpCard == false)
                    {
                        m_LeftManager.StartStakes();
                        m_IsNextPlayerCanStakes = false;
                    }
                }
                //右边玩家下注
                if (m_CurrentStakesIndex % 3 == 2)
                {
                    if (m_RightManager.m_IsGiveUpCard == false)
                    {
                        m_RightManager.StartStakes();
                        m_IsNextPlayerCanStakes = false;
                    }
                }
                m_CurrentStakesIndex++;
            }
        }
    }

    /// <summary>
    /// 下注
    /// </summary>
    /// <param name="count"></param>
    public int Stakes(int count)
    {
        m_LastPlayerStakesCount += count;
        if (m_LastPlayerStakesCount > Models.GameModel.TopStakes)
        {
            m_LastPlayerStakesCount = Models.GameModel.TopStakes;
        }
        return m_LastPlayerStakesCount;
    }

    public void ChooseBanker()
    {
        m_LeftManager.StartChooseBanker();
        m_RightManager.StartChooseBanker();

        int ran = Random.Range(0, 3);
        switch (ran)
        {
            case 0:
                //自身庄家
                m_SelfManager.BecomeBanker();
                m_CurrentSendCardIndex = 0;
                m_CurrentStakesIndex = 1;
                break;
            case 1:
                //左家庄家
                m_LeftManager.BecomeBanker();
                m_CurrentSendCardIndex = 1;
                m_CurrentStakesIndex = 2;
                break;
            case 2:
                //右家庄家
                m_RightManager.BecomeBanker();
                m_CurrentSendCardIndex = 2;
                m_CurrentStakesIndex = 3;
                break;

            default:
                break;

        }
        EventCenter.Broadcast(EventDefine.Hint, "开始发牌");
        StartCoroutine(DealCard());
    }

    private IEnumerator DealCard()
    {
        //1.初始化牌
        if (m_CardList.Count == 0 || m_CardList == null || m_CardList.Count < 9)
        {
            InitCard();
            //2.洗牌
            ClearCard();
        }

        //3.发牌
        for (int i = 0; i < 9; i++)
        {
            if (m_CurrentSendCardIndex % 3 == 0)
            {
                //自身发牌
                m_SelfManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(0, 250, 0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }
            else if (m_CurrentSendCardIndex % 3 == 1)
            {
                //左家发牌
                m_LeftManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(632.5f, 120, 0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }
            else if (m_CurrentSendCardIndex % 3 == 2)
            {
                //右家发牌
                m_RightManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(-632.5f, 120, 0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }
            yield return new WaitForSeconds(m_DealCardDurationTime);
            m_DealCardIndex++;
            m_CurrentSendCardIndex++;
        }

        //发牌结束
        m_SelfManager.DealCardFinished();
        m_RightManager.DealCardFinished();
        m_LeftManager.DealCardFinished();

        m_IsStartStakes = true;
    }

    private void InitCard()
    {
        for (int weight = 2; weight <= 14; weight++)
        {
            for (int color = 0; color <= 3; color++)
            {
                Card card = new Card(weight, color);
                m_CardList.Add(card);
            }
        }
    }

    private void ClearCard()
    {
        for (int i = 0; i < m_CardList.Count; i++)
        {
            int ran = Random.Range(0, m_CardList.Count);
            Card temp = m_CardList[i];
            m_CardList[i] = m_CardList[ran];
            m_CardList[ran] = temp;
        }
    }

    /// <summary>
    /// 右边玩家比牌
    /// </summary>
    public void RightPlayerCompare()
    {
        if (m_SelfManager.m_IsGiveUpCard)
        {
            //和左边玩家比牌
        }
        else
        {
            //和Self比牌
        }
    }

    /// <summary>
    /// 左边玩家比牌
    /// </summary>
    public void LeftPlayerCompare()
    {
        if (m_SelfManager.m_IsGiveUpCard)
        {
            //和右边玩家比牌
        }
        else
        {
            //和Self比牌
        }
    }
}
