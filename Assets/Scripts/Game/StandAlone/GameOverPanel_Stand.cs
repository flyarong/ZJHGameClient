﻿using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverPanel_Stand : MonoBehaviour {

    [System.Serializable]
    public class Player
    {
        public Text txt_CoinCount;
        public Image img_Win;
        public Image img_Lose;
    }

    public Player m_LeftPlayer;
    public Player m_SelfPlayer;
    public Player m_RightPlayer;

    private Button btn_Again;
    private Button btn_MainMenu;
    private AudioSource m_AudioSource;
    public AudioClip clip_Win;
    public AudioClip clip_Lose;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        EventCenter.AddListener<int, int, int>(EventDefine.GameOver, GameOver);
        btn_Again = transform.Find("btn_Again").GetComponent<Button>();
        btn_MainMenu = transform.Find("btn_MainMenu").GetComponent<Button>();

        btn_Again.onClick.AddListener(OnAgainButtonClick);
        btn_MainMenu.onClick.AddListener(OnMainMenuButtonClick);
    }

    private void OnDetroy()
    {
        EventCenter.RemoveListener<int, int, int>(EventDefine.GameOver, GameOver);
    }

    private void OnAgainButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    private void GameOver(int leftCoinCount,int selfCoinCount, int rightCoinCount)
    {
        transform.DOScale(Vector3.one, 0.3f);

        m_LeftPlayer.img_Lose.gameObject.SetActive(false);
        m_LeftPlayer.img_Win.gameObject.SetActive(false);
        m_SelfPlayer.img_Lose.gameObject.SetActive(false);
        m_SelfPlayer.img_Win.gameObject.SetActive(false);
        m_RightPlayer.img_Lose.gameObject.SetActive(false);
        m_RightPlayer.img_Win.gameObject.SetActive(false);


        if (leftCoinCount < 0)
        {
            m_LeftPlayer.img_Lose.gameObject.SetActive(true);
            m_LeftPlayer.txt_CoinCount.text = leftCoinCount.ToString();
        }
        else
        {
            m_LeftPlayer.img_Win.gameObject.SetActive(true);
            m_LeftPlayer.txt_CoinCount.text = (Mathf.Abs(selfCoinCount + rightCoinCount) + leftCoinCount).ToString();
        }

        if (selfCoinCount < 0)
        {
            m_AudioSource.clip = clip_Lose;
            m_AudioSource.Play();
            m_SelfPlayer.img_Lose.gameObject.SetActive(true);
            m_SelfPlayer.txt_CoinCount.text = selfCoinCount.ToString();
        }
        else
        {
            m_AudioSource.clip = clip_Win;
            m_AudioSource.Play();
            int winCoin = Mathf.Abs(leftCoinCount + rightCoinCount) + selfCoinCount;
            if (NetMsgCenter.Instance != null)
            {
                NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, winCoin);
            }
            m_SelfPlayer.img_Win.gameObject.SetActive(true);
            m_SelfPlayer.txt_CoinCount.text = winCoin.ToString();
        }

        if (rightCoinCount < 0)
        {

            m_RightPlayer.img_Lose.gameObject.SetActive(true);
            m_RightPlayer.txt_CoinCount.text = rightCoinCount.ToString();
        }
        else
        {
            m_RightPlayer.img_Win.gameObject.SetActive(true);
            m_RightPlayer.txt_CoinCount.text = (Mathf.Abs(leftCoinCount + selfCoinCount) + rightCoinCount).ToString();
        }

    }
}
