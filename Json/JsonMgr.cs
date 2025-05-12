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
/// Json���ݹ�����
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;

    private JsonMgr() { }

    //�洢����
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //�洢·��
        string path = Application.persistentDataPath + "/" + fileName + ".json";

        //���л� �õ�Json�ַ���
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

        //�����л���Json�ַ��� �洢��ָ��·���ļ���
        File.WriteAllText(path, jsonStr);
    }

    //�������ݷ��� Where T:new()��ȷ��T�������޲ι��� ��Ϊ������ȡ��T���Ͷ�����
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //�ҵ��ļ� �ж��ļ��Ƿ����
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            return new T();//����һ��Ĭ�϶���

        //���з����л�
        string jsonStr = File.ReadAllText(path);
        //���ݶ���
        T data = default(T);
        //�����л�
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
