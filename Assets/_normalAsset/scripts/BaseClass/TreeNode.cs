using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 树节点：要写成类，不可以写成结构体，否则会报错（循环问题）
///【作为模板，一般不会直接使用】
/// 不可序列化其父（会造成无限循环问题）
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class TreeNode<T>
{
   
    private T data;
    //private TreeNode<T> parent;
    private List<TreeNode<T>> childs;
    public List<TreeNode<T>> Childs
    {
        get
        {
            if (childs == null)
                childs = new List<TreeNode<T>>();
            return childs;
        }

        set
        {
            childs = value;
        }
    }

    public T Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }

    //public TreeNode<T> Parent
    //{
    //    get
    //    {
    //        return parent;
    //    }

    //    set
    //    {
    //        parent = value;
    //    }
    //}

    public void AddNode(TreeNode<T> _node)
    {
        //_node.Parent = this;//添加这行会报错（循环问题）
        Childs.Add(_node);
    }

    public void DeleteNode(TreeNode<T> _node)
    {
        if (Childs.Contains(_node))
        {
            Childs.Remove(_node);
            _node = null;
        }
    }

    //将该节点移动到目标节点下,并设置其序号
    public void MoveNodeTo(TreeNode<T> _targetNode, int _index)
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