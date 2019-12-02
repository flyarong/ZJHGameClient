using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSPanel_Stand : MonoBehaviour {

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

    private void VS(BaseManager_Stand compare,BaseManager_Stand compared,string compareName,string comparedName)
    {
        m_ComparePlayer.img_Lose.gameObject.SetActive(false);
        m_ComparePlayer.img_Win.gameObject.SetActive(false);
        m_ComparedPlayer.img_Lose.gameObject.SetActive(false);
        m_ComparedPlayer.img_Win.gameObject.SetActive(false);

        m_ComparePlayer.txt_Name.text = compareName;
        m_ComparedPlayer.txt_Name.text = comparedName;

        for (int i = 0; i < compare.m_CardList.Count; i++)
        {
            string cardName = "card" + compare.m_CardList[i].Color + "_" + compare.m_CardList[i].Weight;
            m_ComparePlayer.cardsArr[i].sprite = ResourceManager.LoadCardSprite(cardName);
        }

        for (int i = 0; i < compared.m_CardList.Count; i++)
        {
            string cardName = "card" + compared.m_CardList[i].Color + "_" + compared.m_CardList[i].Weight;
            m_ComparedPlayer.cardsArr[i].sprite = ResourceManager.LoadCardSprite(cardName);
        }

        CompareCard(compare, compared);
    }

    /// <summary>
    /// 比牌的逻辑算法
    /// </summary>
    /// <param name="compare"></param>
    /// <param name="compard"></param>
    private void CompareCard(BaseManager_Stand compare,BaseManager_Stand compard)
    {

    }
}
