using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 分形框架：从最初的一个点展开出若干枝叶，每个枝叶又能继续展开，也能从任意枝叶开始回溯
/// 功能：展开，聚焦，收缩，暂存，插入，关联
/// </summary>
public class FractalFrame : MonoBehaviour
{
    #region Parameters
    public static FractalFrame _instance;
    FractalNode treeRoot;
    string savedataPath = "Assets/_Use/Fractal/FractalData";
    string nodePrefabPath = "Prefabs/FractalObj";
    public Dictionary<int, FractalObj> nodes = new Dictionary<int, FractalObj>();
    public float spreadingTime = 1;
    public float focusTime = 1;
    public float contractTime = 0.1f;
    public float spreadRadius = 4;
    public Vector3 focusOnDir = new Vector3();
    [HideInInspector]
    public FractalObj curFocusObj;
    #endregion
    #region Properties
    #endregion
    #region Private Methods       
    private void Awake()
    {
        _instance = this;
        InitRoot();        
    }
    private void Start()
    {
        ShowRoot();
    }

    /// <summary>
    /// 需要通过测试，获取序列化后的格式，从而根据格式扩展结构
    /// </summary>
    [ContextMenu("Test")]
    void TestRoot()
    {
        treeRoot = new FractalNode();
        var child0 = new FractalNode();
        var child1 = new FractalNode();
        var child00 = new FractalNode();
        var child01 = new FractalNode();
        var child000 = new FractalNode();
        var child001 = new FractalNode();
        var child002 = new FractalNode();
        var child10 = new FractalNode();
        var child11 = new FractalNode();
        var child100 = new FractalNode();
        var child110 = new FractalNode();
        var _step = 1;
        treeRoot.AddNode(child0, _step++);
        treeRoot.AddNode(child1, _step++);
        child0.AddNode(child00, _step++);
        child0.AddNode(child01, _step++);
        child00.AddNode(child000, _step++);
        child00.AddNode(child001, _step++);
        child00.AddNode(child002, _step++);
        child1.AddNode(child10, _step++);
        child1.AddNode(child11, _step++);
        child10.AddNode(child100, _step++);
        child11.AddNode(child110, _step++);        
        SerializeTool.SerializeToJson(treeRoot, savedataPath);
    }

    //设置父节点:迭代
    void SetParent(FractalNode _node)
    {
        for (var i = 0; i < _node.Childs.Count; i++)
        {
            _node.Childs[i].parent = _node;
            SetParent(_node.Childs[i]);
        }
    }
    #endregion
    #region Utility Methods
    //读取序列化的文件，反序列化后将root初始化    
    [ContextMenu("Init")]
    public void InitRoot()
    {
        treeRoot = SerializeTool.DeSerializeFromJson<FractalNode>(savedataPath);
        SetParent(treeRoot);
    }

    [ContextMenu("Show")]
    public void ShowRoot()
    {
        ShowNode(0);
        curFocusObj = nodes[0];
    }
    //展开选中的节点:点击节点或按钮时调用
    public void ShowNode(int _id)
    {
        FractalNode _node = SearchNode(treeRoot, _id);
        if (_node == null) return;
        FractalObj _obj;
        Transform _parent = _node.parent!=null?nodes[_node.parent.id]?.transform:null;//
        if (nodes.ContainsKey(_id))
        {
            _obj = nodes[_id];
        }
        else
        {
            _obj = ((GameObject)Instantiate(Resources.Load(nodePrefabPath))).GetComponentInChildren<FractalObj>();
            _obj.id = _id;
            _obj.Trans.SetParent(_parent);
            _obj.name = "FractalObj" + _id;
            _obj.Trans.localPosition = Vector3.zero;
            nodes.Add(_id, _obj);
        }
        for (var i = 0; i < _node.Childs.Count; i++)
        {
            if (!nodes.ContainsKey(_node.Childs[i].id))
            {           
                var _com = ((GameObject)Instantiate(Resources.Load(nodePrefabPath))).GetComponentInChildren<FractalObj>();
                _com.id = _node.Childs[i].id;
                _com.Trans.SetParent(_obj.transform);
                _com.Trans.localPosition = Vector3.zero;
                _com.name = "FractalObj" + _com.id;
                _obj.Childs.Add(_com);
                nodes.Add(_node.Childs[i].id, _com);
            }
        }
        StartCoroutine(_obj.IE_Spreading());

    }   
    //聚焦到选中的节点
    public void FocusNode(int _id)
    {
        StartCoroutine( nodes[_id].IE_Focusing());
    }
    //根据id找到指定的节点
    public FractalNode SearchNode(FractalNode _node, int _id)
    {
        if (_node.id == _id)
        {
            return _node;
        }
        else
        {
           
            for (var i = 0; i < _node.Childs.Count; i++)
            {
                FractalNode _temp = SearchNode(_node.Childs[i], _id);
                if (_temp != null)
                {
                    return _temp;
                }
            }
            return null;
        }
    }
    #endregion






}

