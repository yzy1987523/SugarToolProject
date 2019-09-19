using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;//链接：https://pan.baidu.com/s/1Yxbesci1zRpTdZV1VJSEDQ 提取码：yoyw
/// <summary>
/// 序列化工具：将数据序列化为XML；将XML反序列化为数据
/// </summary>
public class SerializeTool
{
    #region XML
    /// <summary>
    /// 将数据序列化为XML：写入
    /// </summary>
    /// <typeparam name="T">数据类</typeparam>
    /// <param name="_data">数据</param>
    /// <param name="_path">xml保存路径</param>
    public static void SerializeToXML<T>(T _data,string _path)
    {       
        FileStream fs = new FileStream(_path+".xml", FileMode.OpenOrCreate);
        XmlSerializer xml = new XmlSerializer(typeof(T));
        xml.Serialize(fs, _data);
        fs.Close();        
    }
    /// <summary>
    /// 将XML反序列化为数据：读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <returns></returns>
    public static T DeSerializeFromXML<T>(string _path)
    {
        FileStream fs = new FileStream(_path + ".xml", FileMode.Open);
        XmlSerializer bf = new XmlSerializer(typeof(T));
        T _data = (T)bf.Deserialize(fs);
        fs.Close();
        return _data;
    }
    #endregion

    #region 二进制
    /// <summary>
    /// 将数据序列化为二进制：写入
    /// </summary>
    /// <typeparam name="T">数据类</typeparam>
    /// <param name="_data">数据</param>
    /// <param name="_path">二进制文件保存路径</param>
    public static void SerializeToBinary<T>(T _data, string _path)
    {
        FileStream fs = new FileStream(_path + ".data", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, _data);
        fs.Close();
    }
    /// <summary>
    /// 将二进制文件反序列化为数据：读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <returns></returns>
    public static T DeSerializeFromBinary<T>(string _path)
    {
        FileStream fs = new FileStream(_path+".data", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        T _data = (T)bf.Deserialize(fs);
        fs.Close();
        return _data;
    }
    #endregion

    #region Json
    /// <summary>
    /// 将数据序列化为Json文件：写入
    /// </summary>
    /// <typeparam name="T">数据类</typeparam>
    /// <param name="_data">数据</param>
    /// <param name="_path">Json文件保存路径</param>
    public static void SerializeToJson<T>(T _data, string _path)
    {
        var _text = JsonConvert.SerializeObject(_data);
        File.WriteAllText(_path + ".json", _text);
    }
    /// <summary>
    /// 将Json文件反序列化为数据：读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <returns></returns>
    public static T DeSerializeFromJson<T>(string _path)
    {
        var _text = File.ReadAllText(_path + ".json");
        T _data = JsonConvert.DeserializeObject<T>(_text);        
        return _data;
    }
    #endregion
}
