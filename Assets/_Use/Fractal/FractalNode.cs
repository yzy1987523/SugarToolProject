using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 分形结构的节点：不可以作为Object,只能作为存储结构的数据，与TreeNode的区别只是多了个id
/// </summary>
[Serializable]
public class FractalNode
{   
    [NonSerialized]
    public FractalNode parent;
    List<FractalNode> childs;
    public int id;  
    public List<FractalNode> Childs
    {
        get
        {
            if (childs == null)
                childs = new List<FractalNode>();
            return childs;
        }

        set
        {
            childs = value;
        }
    }
    public void AddNode(FractalNode _node,int _id)
    {
        //id = _id;
        _node.id = _id;
        //_node.Parent = this;//添加这行会报错（循环问题）
        Childs.Add(_node);
    }

    public void DeleteNode(FractalNode _node)
    {
        if (Childs.Contains(_node))
        {
            Childs.Remove(_node);
            _node = null;
        }
    }

    //将该节点移动到目标节点下,并设置其序号
    public void MoveNodeTo(FractalNode _targetNode, int _index)
    {
        if (_index >= _targetNode.Childs.Count)
        {
            _targetNode.Childs.Add(this);
        }
        else
        {
            _targetNode.Childs.Insert(_index, this);
        }
    }
}