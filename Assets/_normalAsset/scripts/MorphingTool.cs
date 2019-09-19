using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 一个专门用来Morphing的脚本，要做成单例，放在场景中
/// </summary>
public class MorphingTool : MonoBehaviour
{
    public static MorphingTool _instance;
    private void Awake()
    {
        _instance = this;
    }
    /// <summary>
    /// 一个通用的Morphing函数，可以让对象的指定值在一段时间内达到目标值
    /// </summary>
    /// <param name="_fun">调用的对象必须要有ISetValue接口</param>
    /// <param name="_org">初始值，是个固定长度的数组，但只需要给用到的赋值</param>
    /// <param name="_target">目标值</param>
    /// <param name="_useTime">用时</param>
    /// <param name="_easingType">缓动类型</param>
    /// <returns></returns>
    public IEnumerator IE_Morphing(ISetValue _fun, ValueData[] _org, ValueData[] _target, float _useTime = 0.5f, EasingEquation _easingType = EasingEquation.Linear)
    {
        for (var i = 0; i < _org.Length; i++)
        {
            if (_org[i].use)
            {
                StartCoroutine(IE_OnceMorphing(_fun, i, _org[i], _target[i], _useTime, _easingType));
            }
        }        
        
        yield return new WaitForSeconds(_useTime);
    }

    IEnumerator IE_OnceMorphing(ISetValue _fun, int _index, ValueData _org, ValueData _target, float _useTime , EasingEquation _easingType )
    {
        var _end = false;
        var _timer = 0f;
        var _v = 0f;
        var _cur = new ValueData();
        _cur.data = new float[4];
        var _type=(MorphingValueType)_index;
        while (!_end)
        {
            if (_v >= 1)
            {
                _end = true;
            }
            else
            {
                _timer += Time.deltaTime;
            }
            _v = Mathf.Clamp01(_timer / _useTime);
            DataValueMorphing(ref _cur, _org, _target, _easingType, _v);
            _fun.SetValue(_type, _cur);
            yield return null;
        }
    }

    void DataValueMorphing(ref ValueData _cur, ValueData _org, ValueData _target, EasingEquation _easingType, float _v)
    {
        for (var i = 0; i < 4; i++)
        {
            //思考：通过ref能否给结构体赋值=>可以
            _cur.data[i] = Mathf.LerpUnclamped(_org.data[i], _target.data[i], EasingManager.GetEaseProgress(_easingType, _v));
        }
    }
    /// <summary>
    /// 用来设置初始值和目标值
    /// </summary>
    /// <param name="_v0"></param>
    /// <param name="_v1"></param>
    /// <param name="_v2"></param>
    /// <param name="_v3"></param>
    /// <returns></returns>
    public static float[] GetFloat4(float _v0 = 0, float _v1 = 0, float _v2 = 0, float _v3 = 0)
    {
        var _data = new float[4];
        _data[0] = _v0;
        _data[1] = _v1;
        _data[2] = _v2;
        _data[3] = _v3;
        return _data;
    }
    public static float[] GetFloat4(Vector3 _v3)
    {
        var _data = new float[4];
        _data[0] = _v3.x;
        _data[1] = _v3.y;
        _data[2] = _v3.z;
        return _data;
    }
    public static float[] GetFloat4(Quaternion _v4)
    {
        var _data = new float[4];
        _data[0] = _v4.x;
        _data[1] = _v4.y;
        _data[2] = _v4.z;
        _data[3] = _v4.w;
        return _data;
    }
    public static float[] GetFloat4(Color _v4)
    {     
        var _data = new float[4];
        _data[0] = _v4.r;
        _data[1] = _v4.g;
        _data[2] = _v4.b;
        _data[3] = _v4.a;
        return _data;
    }
    /// <summary>
    /// 转换为V3类型的数据
    /// </summary>
    /// <param name="_v3"></param>
    /// <param name="_valueData"></param>
    /// <returns></returns>
    public static Vector3 GetData(Vector3 _v3, ValueData _valueData)
    {
        _v3.x = _valueData.data[0];
        _v3.y = _valueData.data[1];
        _v3.z = _valueData.data[2];
        return _v3;
    }
    /// <summary>
    /// 转换为Q4类型的数据
    /// </summary>
    /// <param name="_v4"></param>
    /// <param name="_valueData"></param>
    /// <returns></returns>
    public static Quaternion GetData(Quaternion _v4, ValueData _valueData)
    {
        _v4.x = _valueData.data[0];
        _v4.y = _valueData.data[1];
        _v4.z = _valueData.data[2];
        _v4.w = _valueData.data[3];
        return _v4;
    }
    /// <summary>
    /// 转换为float
    /// </summary>
    /// <param name="_valueData"></param>
    /// <returns></returns>
    public static float GetData(ValueData _valueData)
    {
        return _valueData.data[0];
    }
    /// <summary>
    /// 转为color
    /// </summary>
    /// <param name="_v4"></param>
    /// <param name="_valueData"></param>
    /// <returns></returns>
    public static Color GetData(Color _v4,ValueData _valueData)
    {
        _v4.r = _valueData.data[0];
        _v4.g = _valueData.data[1];
        _v4.b = _valueData.data[2];
        _v4.a = _valueData.data[3];
        return _v4;
    }

    #region 使用实例
    ////思考：空结构体是否要初始化=>可以不用——调用空结构体时，获得的值是否有默认值=>是的
    //[ContextMenu("Test")]
    //void Test()
    //{
    //    var _count = Enum.GetValues(typeof(MorphingValueType)).Length;
    //    var _org = new ValueData[_count];
    //    var _target = new ValueData[_count];
    //    var v0 = (int)MorphingValueType.Position;
    //    _org[v0] = new ValueData(GetFloat4());
    //    _target[v0] = new ValueData(GetFloat4(1, 1, 1));
    //    StartCoroutine(IE_Morphing(this, _org, _target));
    //}

    //public void SetValue(MorphingValueType _type, ValueData _value)
    //{    
    //    switch (_type)
    //    {
    //        case MorphingValueType.Position:
    //            transform.position = GetData(transform.position, _value);
    //            break;
    //        case MorphingValueType.localPosition:
    //            break;
    //    }
    //}
    #endregion
}
public struct ValueData
{
    public bool use;
    public float[] data;//有4位  
    public ValueData(float[] _data, bool _use = true)
    {
        use = _use;
        data = _data;
    }
    
}
public enum MorphingValueType
{
    Position,
    localPosition,
    Rotation,
    localRotation,
    localScale,
    color,
    value,
}
