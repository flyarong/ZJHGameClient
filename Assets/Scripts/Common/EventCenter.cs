using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{

    private static Dictionary<EventDefine, Delegate> m_EventTable = new Dictionary<EventDefine, Delegate>();


    private static void OnListenerAdding(EventDefine eventType, Delegate callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        Delegate d = m_EventTable[eventType];
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }

    private static void OnListenerRemoving(EventDefine eventType, Delegate callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
        }
    }

    private static void OnListenerRemoved(EventDefine eventType)
    {
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
    }

    /// <summary>
    /// 添加监听,没有参数
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener(EventDefine eventType, CallBack callBack)
    {
        //以下部分可以提取出来作为一个方法，为了清晰每个参数的监听，并没有这么做，方法为OnListenerAdding
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        Delegate d = m_EventTable[eventType];
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("委托类型不同，需要添加新的委托类型"));
        }
        //以上部分可以提取出来作为一个方法，为了清晰每个参数的监听，并没有这么做
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;

    }

    /// <summary>
    /// 添加监听,一个参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T>(EventDefine eventType, CallBack<T> callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        Delegate d = m_EventTable[eventType];
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("委托类型不同，需要添加新的委托类型"));
        }
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;

    }

    /// <summary>
    /// 添加监听,两个参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, X>(EventDefine eventType, CallBack<T, X> callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        Delegate d = m_EventTable[eventType];
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("委托类型不同，需要添加新的委托类型"));
        }
        m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] + callBack;

    }

    /// <summary>
    /// 移除监听,没有参数
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener(EventDefine eventType, CallBack callBack)
    {
        //以下部分可以提取出来，为了清晰结构，并没有这么做，提取的方法为OnListenerRemoving
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误，没有事件码{0}", eventType));
        }
        //以上部分可以提取出来，为了清晰结构，并没有这么做，提取的方法为OnListenerRemoving
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;

        //以下部分可以提取出来，为了清晰结构，并没有这么做，提取的方法为OnListenerRemoved
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
        //以上部分可以提取出来，为了清晰结构，并没有这么做，提取的方法为OnListenerRemoved
    }

    /// <summary>
    /// 移除监听,一个参数
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T>(EventDefine eventType, CallBack<T> callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误，没有事件码{0}", eventType));
        }
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
    }

    /// <summary>
    /// 移除监听,两个参数
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, X>(EventDefine eventType, CallBack<T, X> callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误，没有事件码{0}", eventType));
        }
        m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] - callBack;
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
    }

    /// <summary>
    /// 广播监听，没有参数
    /// </summary>
    /// <param name="eventType"></param>
    public static void Broadcast(EventDefine eventType)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    /// <summary>
    /// 广播监听，一个参数
    /// </summary>
    /// <param name="eventType"></param>
    public static void Broadcast<T>(EventDefine eventType, T arg)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    /// <summary>
    /// 广播监听，两个参数
    /// </summary>
    /// <param name="eventType"></param>
    public static void Broadcast<T, X>(EventDefine eventType, T arg1, X arg2)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, X> callBack = d as CallBack<T, X>;
            if (callBack != null)
            {
                callBack(arg1, arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
}
