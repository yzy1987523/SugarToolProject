using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 分形结构的实例化对象，用来检测射线和显隐等
/// </summary>
public class FractalObj : MonoBehaviour, ISelectable, ISetValue
{
    #region Parameters
    private List<FractalObj> childs;    
    public int id;
    [HideInInspector]
    public bool isSpreading;//展开的
    //[HideInInspector]
    //public bool isFocusing;//聚焦
    private Transform trans;
    Transform virTrans;//虚拟位置——为保证连线始终于球相连
    LineRenderer line;
    bool isMoving;
    Material mat;
    string matPropertyName = "alpha";
    FractalObj parent;
    public float matChangeTime = 0.5f;
    public float matLowValue = 0.5f;
    public float matLightValue = 1f;
    //public Material mat;
    //public float lineWidth = 0.05f;
    #endregion
    #region Properties
    public List<FractalObj> Childs
    {
        get
        {
            if (childs == null)
                childs = new List<FractalObj>();
            return childs;
        }

        set
        {
            childs = value;
        }
    }

    public Transform Trans
    {
        get
        {
            if (trans == null)
                trans = transform;
            return trans;
        }

        set
        {
            trans = value;
        }
    }

    public LineRenderer Line
    {
        get
        {
            if (line == null)
            {
                line = GetComponentInChildren<LineRenderer>();
                if(line==null)
                    line = gameObject.AddComponent<LineRenderer>();
                line.positionCount=2;                               
            }
            return line;
        }

        set
        {
            line = value;
        }
    }

    public Transform VirTrans
    {
        get
        {
            if (virTrans == null)
            {
                virTrans = (new GameObject()).transform;
                virTrans.SetParent (Trans.parent);
            }
            return virTrans;
        }

        set
        {
            virTrans = value;
        }
    }

    public FractalObj Parent
    {
        get
        {
            if (parent == null)
                parent = Trans.parent?.GetComponent<FractalObj>();
            return parent;
        }

        set
        {
            parent = value;
        }
    }

    public Material Mat
    {
        get
        {
            if (mat == null)
            {
                mat = Trans.GetComponent<MeshRenderer>().material;
                curColor = mat.color;
            }
            return mat;
        }

        set
        {
            mat = value;
        }
    }
    #endregion
    #region Private Methods       
    private void Update()
    {
        //当往外移时，开始绘制连线：于父的连线
        if (isMoving)
        {
            if (id != 0)
            {
                Line.SetPosition(0,Trans.parent.position);
                Line.SetPosition(1, VirTrans.position);
                Trans.position = VirTrans.position;
            }
        }
    }
    Color curColor;
  void MatChange(float _value)
    {
        //Mat.SetFloat(matPropertyName,_value);
        curColor.a = _value;
        Mat.color = curColor;
    }
    float GetMatValue()
    {
        //return Mat.GetFloat(matPropertyName);
        return curColor.a;
    }

    #endregion
    #region Utility Methods
    //展开:前置条件“聚焦”已经满足
    public IEnumerator IE_Spreading()
    {
        isSpreading = true;
        for (var i = 0; i < Childs.Count; i++)
        {
            StartCoroutine(Childs[i].IE_MoveOut(FractalFrame._instance.spreadingTime, UnityEngine.Random.onUnitSphere*FractalFrame._instance.spreadRadius));
        }
        //该节点颜色变浅    
        if(Childs.Count!=0)
            StartCoroutine(IE_ColorChange(matChangeTime, matLowValue));
        //该节点父的子颜色变浅
        if (Parent != null) {
            for (var i = 0; i < Parent.Childs.Count; i++)
            {
                if(Parent.Childs[i].id!=id)
                StartCoroutine(Parent.Childs[i].IE_ColorChange(matChangeTime, matLowValue));
            }
        }
        yield return new WaitForSeconds(FractalFrame._instance.spreadingTime);
    }
    public IEnumerator IE_ColorChange(float _useTime,float _endValue)
    {
        var _count = Enum.GetValues(typeof(MorphingValueType)).Length;
        var _org = new ValueData[_count];
        var _target = new ValueData[_count];
        var v0 = (int)MorphingValueType.value;
        _org[v0] = new ValueData(MorphingTool.GetFloat4(GetMatValue()));
        _target[v0] = new ValueData(MorphingTool.GetFloat4(_endValue));        
        yield return StartCoroutine(MorphingTool._instance.IE_Morphing(this, _org, _target, _useTime));
    }
    //自身被展开，往外运动
    public IEnumerator IE_MoveOut(float _useTime, Vector3 _dir)
    {
        isMoving = true;
        var _count = Enum.GetValues(typeof(MorphingValueType)).Length;
        var _org = new ValueData[_count];
        var _target = new ValueData[_count];
        var v0 = (int)MorphingValueType.localPosition;
        _org[v0] = new ValueData(MorphingTool.GetFloat4());
        _target[v0] = new ValueData(MorphingTool.GetFloat4(_dir));
        var v1 = (int)MorphingValueType.localScale;
        _org[v1] = new ValueData(MorphingTool.GetFloat4(Vector3.zero));
        _target[v1] = new ValueData(MorphingTool.GetFloat4(Vector3.one));
        var v2 = (int)MorphingValueType.value;
        _org[v2] = new ValueData(MorphingTool.GetFloat4(GetMatValue()));
        _target[v2] = new ValueData(MorphingTool.GetFloat4(matLightValue));
        yield return StartCoroutine(MorphingTool._instance.IE_Morphing(this, _org, _target, _useTime));
    }
    //收缩,所有子也会收缩
    public IEnumerator IE_Contract(float _useTime)
    {
        isSpreading = false;
        StartCoroutine(IE_ColorChange(matChangeTime, matLightValue));
        for (var i = 0; i < Childs.Count; i++)
        {
            if (Childs[i].isSpreading)
            {
                yield return StartCoroutine(Childs[i].IE_Contract(_useTime / Childs[i].Childs.Count));
            }
            yield return StartCoroutine(Childs[i].IE_MoveIn(_useTime / Childs.Count));
        }
    }
    //自身被收缩，往中心运动
    public IEnumerator IE_MoveIn(float _useTime)
    {
        var _count = Enum.GetValues(typeof(MorphingValueType)).Length;
        var _org = new ValueData[_count];
        var _target = new ValueData[_count];
        var v0 = (int)MorphingValueType.localPosition;
        _org[v0] = new ValueData(MorphingTool.GetFloat4(VirTrans.localPosition));
        _target[v0] = new ValueData(MorphingTool.GetFloat4(Vector3.zero));
        var v1= (int)MorphingValueType.localScale;
        _org[v1] = new ValueData(MorphingTool.GetFloat4(Trans.localScale));
        _target[v1] = new ValueData(MorphingTool.GetFloat4(Vector3.zero));
        yield return StartCoroutine(MorphingTool._instance.IE_Morphing(this, _org, _target, _useTime));
        isMoving = false;
    }
    //聚焦
    public IEnumerator IE_Focusing()
    {
        CamCtrl._instance.SetTargetTrans(Trans.position);
        yield return StartCoroutine( CamCtrl._instance.IE_CamMorphing());
        FractalFrame._instance.curFocusObj = this;
        //isFocusing = true;
        if (!isSpreading)
        {
            FractalFrame._instance.ShowNode(id);
        }
    }

    public void QuitSelect()
    {

    }

    public void Select()
    {
        if (isSpreading)
        {
            if (FractalFrame._instance.curFocusObj==this)
            {
                //收缩
                StartCoroutine(IE_Contract(FractalFrame._instance.contractTime));
            }
            else
            {
                //聚焦
                FractalFrame._instance.FocusNode(id);
            }
        }
        else
        {
            if (FractalFrame._instance.curFocusObj == this)
            {
                //展开
                FractalFrame._instance.ShowNode(id);

            }
            else
            {
                //聚焦同时展开（展开在Focus后进行）
                FractalFrame._instance.FocusNode(id);
            }
        }
    }

    public void SetValue(MorphingValueType _type, ValueData _value)
    {
        switch (_type)
        {
            case MorphingValueType.Position:
                break;
            case MorphingValueType.localPosition:
                VirTrans.localPosition = MorphingTool.GetData(VirTrans.localPosition ,_value);
                break;
            case MorphingValueType.Rotation:
                break;
            case MorphingValueType.localRotation:
                break;
            case MorphingValueType.localScale:
                Trans.localScale = MorphingTool.GetData(Trans.localScale, _value);
                break;
            case MorphingValueType.color:
                break;
            case MorphingValueType.value:
                MatChange(MorphingTool.GetData( _value));
                break;
        }
}
    //缓存一会儿：子收回，连线断开
    public void SaveMoment()
    {

    }
    //将缓存的节点拖入到新的位置
    public void Insert()
    {

    }
    #endregion
}
