using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Objects/PackageTable", fileName ="PackageTable")]
public class PackageTable : ScriptableObject, ISerializationCallbackReceiver
{
    public static PackageTable _instance;
    public static PackageTable Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Resources.Load<PackageTable>("packageTable");
            }
            return _instance;
        }
    }

    public List<PackageTableItem> DataList = new List<PackageTableItem>();

    private Dictionary<int, PackageTableItem> _dataDic = new Dictionary<int, PackageTableItem>();

    public PackageTableItem GetItemByID(int id)
    {
        if (_dataDic.TryGetValue(id, out PackageTableItem item))
        {
            return item;
        }
        return null;
    }


    public void OnAfterDeserialize()
    {
        _dataDic.Clear();
        foreach (var item in DataList)
        {
            if (!_dataDic.ContainsKey(item.id))
            {
                _dataDic.Add(item.id, item);
            }
        }
    }

    public void OnBeforeSerialize() { }
}

[Serializable]
public class PackageTableItem
{
    public int id;
    public string name;
    public string type;
    public Sprite image;
    [TextArea] public string details;
    public GameObject prefabs;
}