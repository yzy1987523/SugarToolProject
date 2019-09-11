using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 通用工具类
/// 1.权重随机：根据可选项的权重随机返回一个选项
/// </summary>
public class NormalTool
{
    //权重随机：_roll是0-1的随机数
    public static int GetRandomIndex(float[] _optionWeights,float _roll)
    {        
        float sum = 0f;
        for (var i = 0; i < _optionWeights.Length; i++)
        {
            sum += _optionWeights[i];
        }
        var a = new float[_optionWeights.Length];
        for (int j = 0; j < a.Length; j++)
        {
            a[j] /= sum;
        }
        int n = 0;
        var x = 0f;
        while (n < a.Length)
        {
            x += a[n];
            if (_roll <= x)
            {
                return n;
            }
            n++;
        }
        return 0;
    }
}
