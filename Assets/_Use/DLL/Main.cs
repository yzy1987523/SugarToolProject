using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Reflection;
using UnityEngine.Networking;
public class Main : MonoBehaviour
{
    public bool isUpdate = false;
    Assembly assembly;
    void Start()
    {
        if (isUpdate)
        {
            //更新启动
            Debug.Log("更新启动");
            //StartCoroutine(LoadDll());
        }
        else
        {
            //正常启动
            Debug.Log("正常启动");
            //gameObject.AddComponent<DllManager>().InitAssembly(isUpdate, null);
        }
    }
    void LoadDll()
    {
        //string path = "Assets/AssetBundles/Dll1";
        //WWW www = new WWW(path);
        //yield return www;
        AssetBundle bundle = GetBundle("Dll1");
        TextAsset asset = bundle.LoadAsset("Dll1", typeof(TextAsset)) as TextAsset;

        assembly = Assembly.Load(asset.bytes);
        Type type = assembly.GetType("Dll1");
        var com = gameObject.AddComponent(type);

        MethodInfo methodInfo = type.GetMethod("yzy");
        methodInfo.Invoke(com, new object[] {  });
    }
    public AssetBundle GetBundle(string _name)
    {
        return AssetBundle.LoadFromMemory(System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/AssetBundles/" + _name.ToLower()));
    }
}