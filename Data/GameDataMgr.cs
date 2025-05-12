using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据管理类 单例模式
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //音乐音效数据
    public MusicData musicData;
    //怪物数据
    public List<MonsterInfo> monsterData;
    //玩家数据
    public List<PlayerInfo> playerData;

    private GameDataMgr()
    {
        //一开始就加载音乐音效数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //一开始就加载怪兽数据
        monsterData = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        //一开始就加载玩家数据
        playerData = JsonMgr.Instance.LoadData<List<PlayerInfo>>("PlayerInfo");
    }
}
