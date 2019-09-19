using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 一个比较通用的扩展方法类
/// </summary>
public static class ExpandMethods
{
    public static bool IsClose(this Vector3 self, Vector3 target, float dis)
    {
        return self.GetHorizontalDistance(target) <= dis;
    }
    //批量显隐蒙皮网格
    public static void ShowModel(this SkinnedMeshRenderer[] meshRenderer, bool show)
    {
        foreach (var mesh in meshRenderer)
        {
            mesh.enabled = show;
        }
    }
    //获取水平方向的距离
    public static float GetHorizontalDistance(this Vector3 self, Vector3 target)
    {
        target.y = self.y;
        return Vector3.Distance(target, self);
    }
    //获取直线距离
    public static float GetDistance(this Vector3 self, Vector3 target)
    {
        return Vector3.Distance(target, self);
    }
    //水平方向上朝向目标点
    public static void LookAtHorizontal(this Transform self, Vector3 target)
    {
        target.y = self.position.y;
        self.LookAt(target);
    }
    //获取在self到target方向上，距离target有dis长的点，其中默认为水平方向
    public static Vector3 GetPointCloseTarget(this Vector3 self, Vector3 target, float dis, bool isHorizontal = true)
    {
        if (isHorizontal)
        {
            target.y = self.y;
        }
        var dir = target - self;
        return target - dir.normalized * dis;
    }
    //获取在self到target方向上，距离self有dis长的点，其中默认为水平方向
    public static Vector3 GetPointByDir(this Vector3 self, Vector3 dir, float dis, bool isHorizontal = true)
    {
        if (isHorizontal)
        {
            dir.y = self.y;
        }
        return self + dir.normalized * dis;
    }
    //获取点在水平面上的倒影
    public static Vector3 PosOfReverseY(this Vector3 self)
    {
        var _pos = self;
        _pos.y = -_pos.y;
        return _pos;
    }
    //顺时针时更近
    public static bool IsCloseClockwise(this Vector3 _from, Vector3 _to)
    {
        var _rot = Quaternion.FromToRotation(_from, _to);
        return _rot.eulerAngles.y < 180;
    }
    public static void SetPosToSameHeight(this Transform _trans, Vector3 _target)
    {
        var _p = _target;
        _p.y = _trans.position.y;
        _trans.position = _p;
    }
    //点在对象前方一定角度内
    public static bool IsInFrontArea(this Transform _trans, Vector3 _target, float _angle)
    {
        return (Vector3.Angle(_target - _trans.position, _trans.forward) <= _angle);
    }
    //弹簧:this与_mid差距越大，返回值差值越大,_resilience=0.02f比较好，_mid一般都为0
    public static float ChangeBySpring(this float _this, float _resilience, float _mid)
    {
        return _this - (_this - _mid) * _resilience;
    }

    public static bool IsSameAnimState(this Animator _this, int _hash)
    {
        return _this.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(_hash);
    }
    //获取夹角（按在以水平面上的情况,从-180到180）
    public static float GetAngleByHorizontal(this Vector3 _from, Vector3 _to)
    {
        if (Vector3.Cross(_from, _to).y > 0)
        {
            return -Vector3.Angle(_from, _to);//在左侧
        }
        return Vector3.Angle(_from, _to);//在右侧
    }
    //获取字符串后面的数字，假如没有数字则返回为null
    public static int? GetLastNum(this string _v)
    {
        var match = System.Text.RegularExpressions.Regex.Match(_v, "[0-9]+$");
        if (match.Length > 0)
        {
            return int.Parse(match.Value);
        }
        return null;

    }
    // 多用于旋转控制时，用于限制角度
    public static float ClampAngle(this float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
