using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 贝塞尔曲线应用
/// </summary>

public class BezierCurve : MonoBehaviour
{
    public Transform[] transes;
    Vector3[] points;
    public float useTime=2;
    public EasingEquation easeType;
    
    Vector3[] Points
    {
        get
        {
            if(points==null)
            {
                points = new Vector3[transes.Length];
                for(var i = 0; i < transes.Length; i++)
                {
                    points[i] = transes[i].position;
                }
            }
            return points;
        }

        set
        {
            points = value;
        }
    }
    public void SetPoints(Vector3[] _points)
    {
        points = _points;
    }
    [ContextMenu("Play")]
    public void Play()
    {
        StartCoroutine(IE_Play());

    }
    IEnumerator IE_Play()
    {
        var _end = false;
        var _v = 0f;
        var _timer = 0f;
        var _useTime = useTime;
        var _orgV1 = 0f;
        var _endV1 = 1f;
        var _curV1 = _orgV1;
        while (!_end)
        {
            _timer += Time.deltaTime;
            _v = Mathf.Clamp01(_timer / _useTime);
            if (_v >= 1)
            {
                _end = true;
            }
            _curV1 = Mathf.LerpUnclamped(_orgV1, _endV1, EasingManager.GetEaseProgress(easeType,_v));
            transform.position = bezier_interpolation_func(_curV1, Points);
            yield return null;
        }
    }  
    private float calc_combination_number(int n, int k)
    {
        float[] result = new float[n + 1];
        for (int i = 1; i <= n; i++)
        {
            result[i] = 1;
            for (int j = i - 1; j >= 1; j--)
                result[j] += result[j - 1];
            result[0] = 1;
        }
        return result[k];
    }
    //根据指定路径返回贝塞尔轨迹
    private Vector3 bezier_interpolation_func(float t, Vector3[] points)
    {
        var count = points.Length;
        Vector3 PointF = new Vector3();
        float[] part = new float[count];
        float sum_x = 0, sum_y = 0,sum_z=0;
        for (int i = 0; i < count; i++)
        {
            float tmp;
            int n_order = count - 1;    // 阶数
            tmp = calc_combination_number(n_order, i);
            sum_x += (float)(tmp * points[i].x * Mathf.Pow((1 - t), n_order - i) * Mathf.Pow(t, i));
            sum_y += (float)(tmp * points[i].y * Mathf.Pow((1 - t), n_order - i) * Mathf.Pow(t, i));
            sum_z += (float)(tmp * points[i].z * Mathf.Pow((1 - t), n_order - i) * Mathf.Pow(t, i));
        }
        PointF.x = sum_x;
        PointF.y = sum_y;
        PointF.z = sum_z;
        return PointF;
    }
}
