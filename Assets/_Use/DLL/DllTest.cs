using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Reflection;

public class DllTest : MonoBehaviour
{
    [DllImport("Dll2", EntryPoint = "Add")]
    extern static int Add(int a, int b);

    // Start is called before the first frame update
    void Start()
    {
        int num = Add(1, 1);
        Debug.Log(num);
    }
    void T1()
    {
        var p = @"D:\Document\Unity\TestDll\bin\Release\TestDll.dll";
        byte[] bin;
        using (var fs = new FileStream(p, FileMode.Open))
        {
            using (var br = new BinaryReader(fs))
            {
                bin = br.ReadBytes(Convert.ToInt32(fs.Length));
            }
        }
        var assembly = Assembly.Load(bin);

        var t = assembly.GetType("TestDll.test");
        var instance = Activator.CreateInstance(t);
        var m = t.GetMethod("add");
        Debug.Log(m.Invoke(instance, new object[] { 1, 2 }));
    }
}
