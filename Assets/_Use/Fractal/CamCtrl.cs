using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour,ISetValue
{
    public static CamCtrl _instance;
    public float camMoveTime;
    private Transform camTrans;
    private Transform targetTrans;
    Vector3 dirWithTarget=new Vector3(0,0,-1);//摄像机会绕该点转动，该点与焦点位置重合
    public float orgDisWithTarget=10;//默认情况下，摄像机与目标点的距离
    public float curDisWithTarget;
    Quaternion orgRotWithTarget;//默认是没有旋转的，但是有旋转的话就需要该参数了
    Quaternion curRotWithTarget;

    public Transform CamTrans
    {
        get
        {
            if (camTrans == null)
                camTrans = transform;
            return camTrans;
        }

        set
        {
            camTrans = value;
        }
    }

    public Transform TargetTrans
    {
        get
        {
            if (targetTrans == null) {
                targetTrans = (new GameObject()).transform;
                targetTrans.name = "CamTargetTrans";
            }
            return targetTrans;
        }

        set
        {
            targetTrans = value;
        }
    }

    private void Awake()
    {
        _instance = this;
        Init();
    }
    void Init()
    {
        curDisWithTarget = orgDisWithTarget;
    }
    
    public void SetTargetTrans(Vector3 _pos)
    {
        TargetTrans.position = _pos;
        //TargetTrans.rotation = _rot;
    }    
    
    public void Rot(Quaternion _variableOfRot)
    {      
        curRotWithTarget = _variableOfRot;        
        CamTrans.position = TargetTrans.position + curRotWithTarget * dirWithTarget * curDisWithTarget;
        CamTrans.rotation = _variableOfRot;
    }
    public void Scroll(float _variableOfDis)
    {
        curDisWithTarget = orgDisWithTarget + _variableOfDis;
        CamTrans.position = TargetTrans.position + curRotWithTarget*dirWithTarget*curDisWithTarget;
    }

    public IEnumerator IE_CamMorphing()
    {
        var _count = Enum.GetValues(typeof(MorphingValueType)).Length;
        var _org = new ValueData[_count];
        var _target = new ValueData[_count];
        var v0 = (int)MorphingValueType.Position;
        _org[v0] = new ValueData(MorphingTool.GetFloat4(CamTrans.position));
        _target[v0] = new ValueData(MorphingTool.GetFloat4(TargetTrans.position + curRotWithTarget * dirWithTarget * curDisWithTarget));
        //var v1 = (int)MorphingValueType.Rotation;
        //_org[v1] = new ValueData(MorphingTool.GetFloat4(CamTrans.rotation));
        //_target[v1] = new ValueData(MorphingTool.GetFloat4(TargetTrans.rotation));
        yield return StartCoroutine(MorphingTool._instance.IE_Morphing(this, _org, _target, camMoveTime));
    }
    public void SetValue(MorphingValueType _type, ValueData _value)
    {
        switch (_type)
        {
            case MorphingValueType.Position:
                CamTrans.position = MorphingTool.GetData(CamTrans.position, _value);
                break;
            case MorphingValueType.localPosition:
                CamTrans.localPosition = MorphingTool.GetData(CamTrans.localPosition, _value);
                break;
            case MorphingValueType.Rotation:
                CamTrans.rotation = MorphingTool.GetData(CamTrans.rotation, _value);
                break;
            case MorphingValueType.localRotation:
                break;
            case MorphingValueType.localScale:
                break;
            case MorphingValueType.color:
                break;
            case MorphingValueType.value:
                break;
        }
    }
}
