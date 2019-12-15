using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VSPanel_Stand : MonoBehaviour {

    public float m_DealyTime = 2f;

    [System.Serializable]
    public class Player
    {
        public Text txt_Name;
        public Image[] cardsArr;
        public Image img_Lose;
        public Image img_Win;

    }

    public Player m_ComparePlayer;
    public Player m_ComparedPlayer;

    public BaseManager_Stand compare;
    public BaseManager_Stand compared;

    private void Awake()
    {
        EventCenter.AddListener<BaseManager_Stand, BaseManager_Stand>(EventDefine.VSAI, CompareCard);
        EventCenter.AddListener<BaseManager_Stand, BaseManager_Stand,string,string>(EventDefine.VSWithSelf, VS);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<BaseManager_Stand, BaseManager_Stand>(EventDefine.VSAI, CompareCard);
        EventCenter.RemoveListener<BaseManager_Stand, BaseManager_Stand, string, string>(EventDefine.VSWithSelf, VS);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(m_DealyTime);
        transform.DOScale(Vector3.zero, 0.3f);
    }

    IEnumerator CompareWin()
    {
        yield return new WaitForSeconds(m_DealyTime);
        compare.Win();
        compared.Lose();
    }

    IEnumerator CompareLose()
    {
        yield return new WaitForSeconds(m_DealyTime);
        compare.Lose();
        compared.Win();
    }


    private void VS(BaseManager_Stand compare,BaseManager_Stand compared,string compareName,string comparedName)
    {
        transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
        {
            StartCoroutine(Delay());
        });
        m_ComparePlayer.img_Lose.gameObject.SetActive(false);
        m_ComparePlayer.img_Win.gameObject.SetActive(false);
        m_ComparedPlayer.img_Lose.gameObject.SetActive(false);
        m_ComparedPlayer.img_Win.gameObject.SetActive(false);

        m_ComparePlayer.txt_Name.text = compareName;
        m_ComparedPlayer.txt_Name.text = comparedName;

        for (int i = 0; i < compare.m_CardList.Count; i++)
        {
            string cardName = "card_" + compare.m_CardList[i].Color + "_" + compare.m_CardList[i].Weight;

            m_ComparePlayer.cardsArr[i].sprite = ResourceManager.LoadCardSprite(cardName);
        }

        for (int i = 0; i < compared.m_CardList.Count; i++)
        {
            string cardName = "card_" + compared.m_CardList[i].Color + "_" + compared.m_CardList[i].Weight;
            m_ComparedPlayer.cardsArr[i].sprite = ResourceManager.LoadCardSprite(cardName);
        }

        CompareCard(compare, compared);
    }

    /// <summary>
    /// 比牌的逻辑算法
    /// </summary>
    /// <param name="compare"></param>
    /// <param name="compard"></param>
    private void CompareCard(BaseManager_Stand compare,BaseManager_Stand compared)
    {
        this.compare = compare;
        this.compared = compared;
        if (compare.m_CardType > compared.m_CardType)
        {
            StartCoroutine(CompareWin());//比较者胜利
            m_ComparePlayer.img_Win.gameObject.SetActive(true);
            m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
        }
        else if (compare.m_CardType == compared.m_CardType)
        {
            if (compare.m_CardType == CardType.Min)
            {
                CompareDanCard(compare,compared);
            }
            else if (compare.m_CardType == CardType.Duizi)
            {
                int compareDuiziValue = 0;
                int compareDanValue = 0;
                int comparedDuiziValue = 0;
                int comparedDanValue = 0;
                //比较者
                if (compare.m_CardList[0].Weight == compare.m_CardList[1].Weight)
                {
                    compareDuiziValue = compare.m_CardList[0].Weight;
                    compareDanValue = compare.m_CardList[2].Weight;
                }
                else if (compare.m_CardList[1].Weight == compare.m_CardList[2].Weight)
                {
                    compareDuiziValue = compare.m_CardList[1].Weight;
                    compareDanValue = compare.m_CardList[0].Weight;
                }
                //被比较者
                if (compared.m_CardList[0].Weight == compared.m_CardList[1].Weight)
                {
                    comparedDuiziValue = compared.m_CardList[0].Weight;
                    comparedDanValue = compared.m_CardList[2].Weight;
                }
                else if (compared.m_CardList[1].Weight == compared.m_CardList[2].Weight)
                {
                    comparedDuiziValue = compared.m_CardList[1].Weight;
                    comparedDanValue = compared.m_CardList[0].Weight;
                }

                if (compareDuiziValue > comparedDuiziValue)
                {
                    StartCoroutine(CompareWin());//比较者胜利
                    m_ComparePlayer.img_Win.gameObject.SetActive(true);
                    m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
                }
                else if (compareDuiziValue == comparedDuiziValue)
                {
                    if (compareDanValue > comparedDanValue)
                    {
                        StartCoroutine(CompareWin());//比较者胜利
                        m_ComparePlayer.img_Win.gameObject.SetActive(true);
                        m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
                    }
                    else
                    {
                        StartCoroutine(CompareLose());//比较者失败
                        m_ComparePlayer.img_Lose.gameObject.SetActive(true);
                        m_ComparedPlayer.img_Win.gameObject.SetActive(true);
                    }
                }
                else
                {
                    StartCoroutine(CompareLose());//比较者失败
                    m_ComparePlayer.img_Lose.gameObject.SetActive(true);
                    m_ComparedPlayer.img_Win.gameObject.SetActive(true);
                }
            }
            else if (compare.m_CardType == CardType.Shunzi || compare.m_CardType == CardType.Shunjin || compare.m_CardType == CardType.Baozi)
            {
                int compareSum = 0;
                int comparedSum = 0;
                for (int i = 0; i < compare.m_CardList.Count; i++)
                {
                    compareSum += compare.m_CardList[i].Weight;
                }
                for (int i = 0; i < compared.m_CardList.Count; i++)
                {
                    comparedSum += compared.m_CardList[i].Weight;
                }
                if (compareSum > comparedSum)
                {
                    StartCoroutine(CompareWin());//比较者胜利
                    m_ComparePlayer.img_Win.gameObject.SetActive(true);
                    m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
                }
                else
                {
                    StartCoroutine(CompareLose());//比较者失败
                    m_ComparePlayer.img_Lose.gameObject.SetActive(true);
                    m_ComparedPlayer.img_Win.gameObject.SetActive(true);
                }
            }
            else if (compare.m_CardType == CardType.Jinhua)
            {
                CompareDanCard(compare, compared);
            }
            else if (compare.m_CardType == CardType.Max)
            {
                StartCoroutine(CompareLose());//比较者失败
                m_ComparePlayer.img_Lose.gameObject.SetActive(true);
                m_ComparedPlayer.img_Win.gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(CompareLose());//比较者失败
            m_ComparePlayer.img_Lose.gameObject.SetActive(true);
            m_ComparedPlayer.img_Win.gameObject.SetActive(true);
        }
    }

    private void CompareDanCard(BaseManager_Stand compare, BaseManager_Stand compared)
    {
        if (compare.m_CardList[0].Weight > compared.m_CardList[0].Weight)
        {
            StartCoroutine(CompareWin());//比较者胜利
            m_ComparePlayer.img_Win.gameObject.SetActive(true);
            m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
        }
        else if (compare.m_CardList[0].Weight == compared.m_CardList[0].Weight)
        {
            if (compare.m_CardList[1].Weight > compared.m_CardList[1].Weight)
            {
                StartCoroutine(CompareWin());//比较者胜利
                m_ComparePlayer.img_Win.gameObject.SetActive(true);
                m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
            }
            else if (compare.m_CardList[1].Weight == compared.m_CardList[1].Weight)
            {
                if (compare.m_CardList[2].Weight > compared.m_CardList[2].Weight)
                {
                    StartCoroutine(CompareWin());//比较者胜利
                    m_ComparePlayer.img_Win.gameObject.SetActive(true);
                    m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
                }
                else if (compare.m_CardList[2].Weight == compared.m_CardList[2].Weight)
                {
                    StartCoroutine(CompareLose());//比较者失败
                    m_ComparePlayer.img_Lose.gameObject.SetActive(true);
                    m_ComparedPlayer.img_Win.gameObject.SetActive(true);
                }
                else
                {
                    StartCoroutine(CompareLose());//比较者失败
                    m_ComparePlayer.img_Lose.gameObject.SetActive(true);
                    m_ComparedPlayer.img_Win.gameObject.SetActive(true);
                }
            }
            else
            {
                StartCoroutine(CompareLose());//比较者失败
                m_ComparePlayer.img_Lose.gameObject.SetActive(true);
                m_ComparedPlayer.img_Win.gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(CompareLose());//比较者失败
            m_ComparePlayer.img_Lose.gameObject.SetActive(true);
            m_ComparedPlayer.img_Win.gameObject.SetActive(true);
        }
    }
}
