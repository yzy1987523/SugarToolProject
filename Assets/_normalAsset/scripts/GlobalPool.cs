using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 全局对象池： 包含多个子对象池，
/// </summary>
public class GlobalPool
{
    #region Parameters
    private static GlobalPool instance;
    Dictionary<string,List<GameObject>> pools;
    Dictionary<string, GameObject> prefabs;
    string prefabPathHead="Prefabs/";
    #endregion
    #region Properties
    public static GlobalPool Instance
    {
        get
        {
            if (instance == null)
                instance = new GlobalPool();
            return instance;
        }

        set
        {
            instance = value;
        }
    }
    #endregion
    #region Private Methods      

    #endregion
    #region Utility Methods
    public GameObject GetObj(string objName)
    {
        GameObject result = null;
        //判断是否有该名字的对象池      //对象池中有对象
        if (pools.ContainsKey(objName) && pools[objName].Count > 0)
        {
            //获取这个对象池中的第一个对象
            result = pools[objName][0];
            //激活对象
            result.SetActive(true);
            //从对象池中移除对象
            pools[objName].Remove(result);
            //返回结果
            return result;
        }
        //如果没有该名字的对象池或者该名字对象池没有对象
        GameObject Prefab = null;
        if (prefabs.ContainsKey(objName)) //如果已经加载过预设体
        {
            Prefab = prefabs[objName];
        }
        else //如果没有加载过预设体
        {
            //加载预设体
            Prefab = Resources.Load<GameObject>(prefabPathHead + objName);
            //更新预设体的字典
            prefabs.Add(objName, Prefab);
        }
        //实例化物体
        result = UnityEngine.Object.Instantiate(Prefab);
        //改名 去除Clone
        result.name = objName;
        return result;
    }
    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    public void RecycleObj(GameObject Obj)
    {
        Obj.SetActive(false);
        //如果有该对象的对象池，直接放在池子中
        if (pools.ContainsKey(Obj.name))
        {
            pools[Obj.name].Add(Obj);
        }
        else//如果没有该对象的对象池，创建一个该类型的池子，并将对象放入
        {
            pools.Add(Obj.name, new List<GameObject>() { Obj });
        }
    }
    #endregion
}