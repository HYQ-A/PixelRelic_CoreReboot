using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public enum JsonType
{
    LitJson,
    JsonUtility,
}

/// <summary>
/// Json数据管理类
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;

    private JsonMgr() { }

    //存储方法
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";

        //序列化 得到Json字符串
        string jsonStr = "";

        switch (type)
        {
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
            case JsonType.JsonUtility:
                jsonStr = JsonUtility.ToJson(data);
                break;
        }

        //把序列化的Json字符串 存储到指定路径文件中
        File.WriteAllText(path, jsonStr);
    }

    //加载数据方法 Where T:new()是确保T类型有无参构造 因为用来读取的T类型都是类
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //找到文件 判断文件是否存在
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            return new T();//返回一个默认对象

        //进行反序列化
        string jsonStr = File.ReadAllText(path);
        //数据对象
        T data = default(T);
        //反序列化
        switch (type)
        {
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
            case JsonType.JsonUtility:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
        }
        return data;
    }
}
