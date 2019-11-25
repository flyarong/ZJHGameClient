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
}
