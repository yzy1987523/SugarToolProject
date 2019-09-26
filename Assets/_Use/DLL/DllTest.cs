using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Reflection;
using UnityEngine.Networking;
//using ClassLibrary1;
public class DllTest : MonoBehaviour
{
    [DllImport("ClassLibrary1")]
    extern static int Add();

    //[DllImport("Dll1")]
    //extern static int Add(int a,int b);
    //[DllImport("Dll1", EntryPoint = "FreeDll")]
    //extern static void FreeDll();
    //public byte[] bin;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Add(1, 2));
        //int num = Add(1, 1);
        ////Debug.Log(ClassLibrary1.Class1.C_Add(1, 2));

        T1();
        //FreeDll();

    }
    void T1()
    {
        var p = @"D:\Projects\CPlusProjects\ClassLibrary1\ClassLibrary1\bin\Release\ClassLibrary1.dll";
        byte[] bin;
        using (var fs = new FileStream(p, FileMode.Open))
        {
            using (var br = new BinaryReader(fs))
            {
                bin = br.ReadBytes((int)fs.Length);
            }
        }
        var assembly = Assembly.Load(bin);
        var t = assembly.GetType("ClassLibrary1.Class1");
        var instance = Activator.CreateInstance(t);
        var m = t.GetMethod("C_Add");
        Debug.Log(m.Invoke(instance, new object[] { 1, 2 }));
    }
    //void T2()
    //{
    //    var assembly = Assembly.Load(bin);

    //    var t = assembly.GetType("DllTest");
    //    var instance = Activator.CreateInstance(t);
    //    var m = t.GetMethod("yzy");
    //    Debug.Log(m.Invoke(instance, new object[] { 1, 2 }));
    //}

    //IEnumerator LoadDll()
    //{
    //    string path = @"D:\Projects\CPlusProjects\Dll1\x64\Debug\Dll1.dll";
    //    UnityWebRequest www = new UnityWebRequest(path);
    //    yield return www;
    //    //AssetBundle bundle = www.assetBundle;
    //    //TextAsset asset = ww.LoadAsset("MyDLL", typeof(TextAsset)) as TextAsset;

    //    var assembly = Assembly.Load(asset.bytes);
    //    Type type = assembly.GetType("DllManager");
    //    var com = gameObject.AddComponent(type);

    //    MethodInfo methodInfo = type.GetMethod("InitAssembly");
    //    methodInfo.Invoke(com, new object[] { isUpdate, assembly });
    //}
    void LoadDll()
    {
        //string path = "Assets/AssetBundles/Dll1";
        //WWW www = new WWW(path);
        //yield return www;
        AssetBundle bundle = GetBundle("Dll1");
        TextAsset asset = bundle.LoadAsset("Dll1", typeof(TextAsset)) as TextAsset;
        Debug.Log(asset.text);
        var assembly = Assembly.Load(asset.bytes);
        //Type type = assembly.GetType("Dll1");
        //var com = gameObject.AddComponent(type);

        //MethodInfo methodInfo = type.GetMethod("yzy");
        //methodInfo.Invoke(com, new object[] { });
    }
    public AssetBundle GetBundle(string _name)
    {
        return AssetBundle.LoadFromMemory(System.IO.File.ReadAllBytes("Assets/AssetBundles/" + _name.ToLower()));
    }

}
