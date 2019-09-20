using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 通用工具类
/// 1.权重随机：根据可选项的权重随机返回一个选项
/// 2.高斯随机：返回一个符合正态分布的随机值
/// </summary>
public class NormalTool
{
    //权重随机：_roll是0-1的随机数
    public static int GetRandomIndex(float[] _optionWeights, float _roll)
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

    /// <summary>
    /// 所谓高斯随机，即大量随机数最后呈现出正态分布，这是符合正常自然规律的；
    /// 其他的随机方式虽然也是乱序，但是没有这种规律，就会呈现出太假的感觉；
    /// 用处：随机获取大量有自然规律的数据。
    /// 比如170身高占比很高，2米的就占比很小，用高斯随机就能获得漂亮的数据，用平均随机获得的数据就不太符合现实
    /// </summary>
    /// <param name="u">均数</param>
    /// <param name="d">方差</param>
    /// <returns></returns>
    //随机产生一个符合正态分布的数：比如u=170，d=20，即取值为150-190之间，但也会出现少量超出范围的值
    public static float GetGaussianRandom(float u, float d)
    {
        float u1, u2, z, x;
        if (d <= 0)
        {
            return u;
        }
        u1 = Random.value;
        u2 = Random.value;
        z = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
        x = u + d * z;
        return x;

    }
    /// <summary>
    /// 获取一组互斥的数据
    /// </summary>
    /// <param name="length">取值范围是0-length</param>
    /// <param name="n">数量</param>
    /// <returns></returns>
    public static float[] GetMutExRandom(float length, int n)
    {
        var _result = new float[n];
        var _unit = length / n;
        var _radius = _unit * 0.5f;
        for (var i = 0; i < n; i++)
        {
            _result[i] = GetGaussianRandom(_radius + _unit * i, _radius);
        }
        return _result;
    }
    /// <summary>
    /// 获取一组点的中心点
    /// </summary>
    /// <param name="_points"></param>
    /// <returns></returns>
    public static Vector3 GetCenter(Vector3[] _points)
    {
        var _point0 = _points[0];
        var _point1 = _points[0];
        for (var i = 0; i < _points.Length; i++)
        {
            if (_point0.x > _points[i].x)
            {
                _point0.x = _points[i].x;
            }
            if (_point0.y > _points[i].y)
            {
                _point0.y = _points[i].y;
            }
            if (_point0.z > _points[i].z)
            {
                _point0.z = _points[i].z;
            }
            if (_point1.x < _points[i].x)
            {
                _point1.x = _points[i].x;
            }
            if (_point1.y < _points[i].y)
            {
                _point1.y = _points[i].y;
            }
            if (_point1.z < _points[i].z)
            {
                _point1.z = _points[i].z;
            }
        }
        return (_point0 + _point1) * 0.5f;
    }
}
