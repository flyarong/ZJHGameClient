﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StakesCountHint : MonoBehaviour {
    private float hintShowTime = 1.5f;

    private Text hintText;

    private Color color;

    private void Awake()
    {
        hintText = GetComponent<Text>();
        color = hintText.color;
    }

    private void OnDestroy()
    {
    }

    public void Show(string text)
    {
        hintText.text = text;
        hintText.DOColor(new Color(color.r, color.g, color.b, 1), 0.3f).OnComplete(() =>
        {
            StartCoroutine(Dealy());
        });
    }

    IEnumerator Dealy()
    {
        yield return new WaitForSeconds(hintShowTime);
        hintText.DOColor(new Color(color.r, color.g, color.b, 0), 0.3f);
    }
}
