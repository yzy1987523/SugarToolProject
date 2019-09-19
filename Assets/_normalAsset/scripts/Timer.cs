using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 计时器工具
/// 使用：先调用构造函数，再每帧调用TimeIsEnd，时间结束时返回为true，注意要初始化时间
/// </summary>
public class Timer
{
    float targetTime;
    float curTime;
    public Timer(float _time)
    {
        curTime = 0;
        targetTime = _time;
    }
    public void InitTime()
    {
        curTime = 0;
    }
    public bool TimeIsEnd()
    {
        curTime += Time.deltaTime;
        if (curTime >= targetTime)
        {
            return true;
        }
        return false;
    }
}
