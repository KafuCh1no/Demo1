using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PackageLocalItem
{
    public string uid;
    public int id;
    public int num;
    public bool isNew;
}

public class PackageLocalData
{

    private static PackageLocalData _instance;
    public static PackageLocalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Load();
            }
            return _instance;
        }
    }

    private PackageLocalItem targetItem;
    public string headSlot;
    public string chestSlot;
    public string legsSlot;
    public string bootsSlot;
    public string weaponRightSlot;
    public string weaponLeftSlot;
    public PackageTable _packageTable;

    public System.Action OnDataChanged;

    public List<PackageLocalItem> items = new List<PackageLocalItem>();

    private static string SavePath => Application.persistentDataPath + "/inventory.json";

    public void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(SavePath, json);
            Debug.Log("存档成功: " + SavePath);
        }
        catch (Exception e)
        {
            Debug.LogError("存档失败: " + e.Message);
        }
    }

    private static PackageLocalData Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PackageLocalData>(json);
        }
        return new PackageLocalData();
    }

    public void Reset()
    {
        headSlot = null;
        chestSlot = null;
        legsSlot = null;
        bootsSlot = null;
        weaponRightSlot = null;
        weaponLeftSlot = null;
}

    public void Delete(string uid)
    {
        targetItem = items.Find(i => i.uid == uid);

        if (targetItem != null)
        {
            items.Remove(targetItem);
            Save();
            
            Debug.Log($"物品 {uid} 已从数据中移除");
            OnDataChanged?.Invoke();
        }
    }

    public void Equip(string uid)
    {
        targetItem = items.Find(i => i.uid == uid);
        //Debug.Log($"[调试] items列表为空吗? {items == null}");
        //Debug.Log($"[调试] 查找到的物品为空吗? {items.Find(i => i.uid == uid) == null}");
        //Debug.Log($"[调试] PackageTable单例为空吗? {PackageTable.Instance == null}");

        var staticInfo = PackageTable.Instance.GetItemByID(targetItem.id);
        if(weaponRightSlot != null)
        {
            Debug.Log("有东西");
        }

        if(staticInfo.type == "singleweapon")
        {
            weaponRightSlot = uid;
        }

        Save();
        OnDataChanged?.Invoke();

    }

}

