using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    [SerializeField] Image _image;
    private string _currentUid;
    public static EquipSlot Instance{ get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void DisplayImage(string uid)
    {
        //_currentUid = PackageLocalData.Instance.weaponRightSlot;
        _currentUid = uid;
        var itemData = PackageLocalData.Instance.items.Find(i => i.uid == _currentUid);
        var staticInfo = PackageTable.Instance.GetItemByID(itemData.id);

        _image.sprite = staticInfo.image;

    }
}
