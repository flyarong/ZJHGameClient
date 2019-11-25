using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Protocol.Dto;

public class RankListPanel : MonoBehaviour
{

    public GameObject go_ItemPre;
    private Button btn_Close;
    private Transform m_Parent;
    private List<GameObject> listGo = new List<GameObject>();

    private void Awake()
    {
        //gameObject.SetActive(false);
        EventCenter.AddListener(EventDefine.ShowRankListPanel, Show);
        EventCenter.AddListener<RankListDto>(EventDefine.ShowRankListDto, GetRankListDto);

        btn_Close = transform.Find("bg/Image/btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);
        m_Parent = transform.Find("bg/Image/List/ScrollRect/Parent");

    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankListPanel, Show);
        EventCenter.RemoveListener<RankListDto>(EventDefine.ShowRankListDto, GetRankListDto);
    }

    private void OnCloseButtonClick()
    {
        transform.DOScale(Vector3.zero, 0.3f);
    }

    private void Show()
    {
        transform.DOScale(Vector3.one, 0.3f);
    }

    /// <summary>
    /// 得到排行榜传输模型
    /// </summary>
    /// <param name="rankListDto"></param>
    private void GetRankListDto(RankListDto rankListDto)
    {
        if (rankListDto == null)
        {
            return;
        }
        foreach (var go in listGo)
        {
            Destroy(go);
        }
        listGo.Clear();
        for (int i = 0; i < rankListDto.rankList.Count; i++)
        {
            if (rankListDto.rankList[i].UserName == Models.GameModel.userDto.UserName)
            {
                GameObject go = Instantiate(go_ItemPre, m_Parent);
                go.transform.Find("Index/txt_Index").GetComponent<Text>().text = (i + 1).ToString();
                go.transform.Find("txt_UserName").GetComponent<Text>().text = "我";
                go.transform.Find("txt_CoinCount").GetComponent<Text>().text = rankListDto.rankList[i].CoinCount.ToString();
                go.transform.Find("Index/txt_Index").GetComponent<Text>().color = Color.red;
                go.transform.Find("txt_UserName").GetComponent<Text>().color = Color.red;
                go.transform.Find("txt_CoinCount").GetComponent<Text>().color = Color.red;
                listGo.Add(go);
            }
            else
            {
                GameObject go = Instantiate(go_ItemPre, m_Parent);
                go.transform.Find("Index/txt_Index").GetComponent<Text>().text = (i + 1).ToString();
                go.transform.Find("txt_UserName").GetComponent<Text>().text = rankListDto.rankList[i].UserName;
                go.transform.Find("txt_CoinCount").GetComponent<Text>().text = rankListDto.rankList[i].CoinCount.ToString();
                listGo.Add(go);
            }
        }
    }
}
