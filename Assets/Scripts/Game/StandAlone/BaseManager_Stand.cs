using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class BaseManager_Stand : MonoBehaviour {

    public GameObject go_CardPre;

    protected Image img_Banker;
    protected Transform cardPoints;
    protected GameObject go_CountDown;
    protected Text txt_CountDown;
    protected Text txt_StakesSum;
    protected Image img_HeadIcon;

    protected StakesCountHint m_StakesCountHint;
    protected ZJHManager_Stand m_ZJHManager;


    // 生成的3张牌的集合
    protected List<GameObject> go_SpawnCardList = new List<GameObject>();

    // 自身的3张牌
    public List<Card> m_CardList = new List<Card>();

    //牌型
    public CardType m_CardType;

    //牌的间隔
    protected float m_CardPointX = -70f;

    //是否开始下注
    protected bool m_IsStartStakes = false;

    //倒计时
    protected float m_Time = 60f;

    //计时器
    protected float m_Timer = 0.0f;

    //总下注数
    public int m_StakesSum = 0;

    //是否弃牌
    public bool m_IsGiveUpCard = false;

    //抽象方法输
    public abstract void Lose();

    //抽象方法赢
    public abstract void Win();
    
    public void BecomeBanker()
    {
        img_Banker.gameObject.SetActive(true);
    }

    /// <summary>
    /// 开始下注
    /// </summary>
    public virtual void StartStakes()
    {
        m_IsStartStakes = true;
        go_CountDown.SetActive(true);
        txt_CountDown.text = "60";
        m_Time = 60;
    }

    /// <summary>
    /// 下注之后
    /// </summary>
    /// <param name="count"></param>
    /// <param name="str"></param>
    protected virtual void StakesAfter(int count, string str)
    {
        m_StakesCountHint.Show(count + str);
        m_StakesSum += count;
        txt_StakesSum.text = m_StakesSum.ToString();
    }

    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="card"></param>
    /// <param name="duration"></param>
    /// <param name="initPos"></param>
    public virtual void DealCard(Card card, float duration, Vector3 initPos)
    {
        m_CardList.Add(card);
        //GameObject go = Instantiate(go_CardPre, cardPoints);
        //go.GetComponent<RectTransform>().localPosition = initPos;
        //go.GetComponent<RectTransform>().DOLocalMove(new Vector3(m_CardPointX, 0, 0), duration);
        //go_SpawnCardList.Add(go);

    }

    /// <summary>
    /// 手牌排序 从大到小
    /// </summary>
    protected void SortCards()
    {
        for (int i = 0; i < m_CardList.Count - 1; i++)
        {
            for (int j = 0; j < m_CardList.Count - 1 - i; j++)
            {
                if (m_CardList[j].Weight < m_CardList[j + 1].Weight)
                {
                    Card temp = m_CardList[j];
                    m_CardList[j] = m_CardList[j + 1];
                    m_CardList[j + 1] = temp;
                }
            }
        }
    }

    /// <summary>
    /// 获取牌型
    /// </summary>
    protected void GetCardType()
    {
        if (m_CardList[0].Weight == 5 && m_CardList[1].Weight == 3 && m_CardList[2].Weight == 2)
        {
            m_CardType = CardType.Max;
        }
        else if (m_CardList[0].Weight == m_CardList[1].Weight && m_CardList[0].Weight == m_CardList[2].Weight)
        {
            m_CardType = CardType.Baozi;
        }
        else if (m_CardList[0].Color == m_CardList[1].Color && m_CardList[0].Color == m_CardList[2].Color
            && m_CardList[0].Weight == m_CardList[1].Weight + 1 && m_CardList[0].Weight == m_CardList[2].Weight + 2)
        {
            m_CardType = CardType.Shunjin;
        }
        else if (m_CardList[0].Color == m_CardList[1].Color && m_CardList[0].Color == m_CardList[2].Color)
        {
            m_CardType = CardType.Jinhua;
        }
        else if (m_CardList[0].Weight == m_CardList[1].Weight + 1 && m_CardList[0].Weight == m_CardList[2].Weight + 2)
        {
            m_CardType = CardType.Shunzi;
        }
        else if (m_CardList[0].Weight == m_CardList[1].Weight || m_CardList[1].Weight == m_CardList[2].Weight)
        {
            m_CardType = CardType.Duizi;
        }
        else
        {
            m_CardType = CardType.Min;
        }
    }
}
