using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{

    private static Dictionary<string, Sprite> nameSpriteDic = new Dictionary<string, Sprite>();

    /// <summary>
    /// 获取图集
    /// </summary>
    /// <param name="iconName"></param>
    /// <returns></returns>
    public static Sprite GetSprite(string iconName)
    {
        if (nameSpriteDic.ContainsKey(iconName))
        {
            return nameSpriteDic[iconName];
        }
        Sprite[] sprite = Resources.LoadAll<Sprite>("headIcon");
        string[] nameArr = iconName.Split('_');
        Sprite temp = sprite[int.Parse(nameArr[1])];
        nameSpriteDic.Add(iconName, temp);
        return temp;
    }

    /// <summary>
    /// 加载牌的图集
    /// </summary>
    /// <param name="cardName"></param>
    /// <returns></returns>
    public static Sprite LoadCardSprite(string cardName)
    {
        if (nameSpriteDic.ContainsKey(cardName))
        {
            return nameSpriteDic[cardName];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>("poke/" + cardName);
            nameSpriteDic.Add(cardName, sprite);
            return sprite;
        }
    }
}
