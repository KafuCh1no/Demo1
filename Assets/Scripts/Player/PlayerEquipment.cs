using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }
    [SerializeField] private Transform rightHand;

    private PackageTable _packageTable;

    private void Awake()
    {
        Instance = this;
    }

    public void equipmentUpdate(string _currentUid)
    {
        var itemData = PackageLocalData.Instance.items.Find(i => i.uid == _currentUid);

        var staticInfo = PackageTable.Instance.GetItemByID(itemData.id); 
        if( staticInfo.type == "singleweapon")
        {
            Instantiate(staticInfo.prefabs, rightHand);
        }
    }
}
