using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Reflection;
using UnityEngine.Networking;

public class DllManager : MonoBehaviour
{
    public static DllManager Instance;
    public Assembly assembly;
    public bool isUpdate;
    void Awake()
    {
        Instance = this;
    }

    //初始化反射资源
    public void InitAssembly(bool isUpdate, Assembly assembly)
    {
        this.assembly = assembly;
        this.isUpdate = isUpdate;
        LoadMainView();
    }
    //加载主界面
    private void LoadMainView()
    {
        var mainView = Instantiate(Resources.Load("Panel")) as GameObject;
        mainView.transform.SetParent(GameObject.Find("Canvas").transform);
        mainView.transform.localPosition = Vector3.zero;
        AddCompotent(mainView, "MainUI");
    }
    //动态加载脚本
    public Component AddCompotent(GameObject entity, string compotentName)
    {
        if (isUpdate)
        {
            //反射加载
            return entity.AddComponent(assembly.GetType(compotentName));
        }
        else
        {
            //常规加载
            return entity.AddComponent(Type.GetType(compotentName));
        }
    }
}