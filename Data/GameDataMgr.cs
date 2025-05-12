using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ݹ����� ����ģʽ
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //������Ч����
    public MusicData musicData;
    //��������
    public List<MonsterInfo> monsterData;
    //�������
    public List<PlayerInfo> playerData;

    private GameDataMgr()
    {
        //һ��ʼ�ͼ���������Ч����
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //һ��ʼ�ͼ��ع�������
        monsterData = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        //һ��ʼ�ͼ����������
        playerData = JsonMgr.Instance.LoadData<List<PlayerInfo>>("PlayerInfo");
    }
}
