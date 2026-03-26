using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReview : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;

    private GameObject _currentWeapon;

    private void OnEnable()
    {
        PackageLocalData.Instance.OnDataChanged += EquipmentUpdate;
    }

    private void OnDisable()
    {
        PackageLocalData.Instance.OnDataChanged -= EquipmentUpdate;
    }

    private void EquipmentUpdate()
    {
        if(_currentWeapon != null)
        {
            Destroy(_currentWeapon);
        }

        string weaponUid = PackageLocalData.Instance.weaponRightSlot;

        var itemData = PackageLocalData.Instance.items.Find(i => i.uid == weaponUid);
        var staticInfo = PackageTable.Instance.GetItemByID(itemData.id);

        _currentWeapon = Instantiate(staticInfo.prefabs, weaponSocket);
        _currentWeapon.layer = LayerMask.NameToLayer("CharacterPreview");
    }

}
