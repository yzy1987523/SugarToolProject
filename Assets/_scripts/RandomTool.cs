using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 20190909
/// 这是一个随机工具，需要提前设置选项,权重越高，越容易roll出来
/// </summary>
public class RandomTool : MonoBehaviour
{
    public string[] options;
    public float[] optionWeights;

    public int ToRandom()
    {
        var r = Random.value;
        float sum = 0f;
        for(var i=0;i< optionWeights.Length; i++)
        {
            sum += optionWeights[i];
        }
        var a = new float[optionWeights.Length];
        for (int j = 0; j < a.Length; j++)
        {
            a[j] /= sum;
        }
        int n = 0;
        var x = 0f;
        while (n < a.Length)
        {
            x += a[n];
            if (r <= x)
            {
                return n;
            }
            n++;
        }
        return 0;
    }
}
