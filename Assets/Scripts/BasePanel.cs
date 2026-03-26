using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform contentParent;
    public PackageTable configTable;

    private List<SlotUI> _allSlots = new List<SlotUI>();

    private int _minSlots = 44;
    private int _columns = 5;

    private void Awake()
    {

        AdjustSlotCount(_minSlots);
    }

    private void OnEnable()
    {
        PackageLocalData.Instance.OnDataChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDisable()
    {
        PackageLocalData.Instance.OnDataChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        var savedItems = PackageLocalData.Instance.items;
        int itemCount = savedItems.Count;

        int targetTotal;
        if (itemCount < _minSlots)
        {
            targetTotal = _minSlots;
        }
        else
        {

            int remainder = itemCount % _columns;
            targetTotal = (remainder == 0) ? itemCount : itemCount + (_columns - remainder);
        }

        AdjustSlotCount(targetTotal);


        for (int i = 0; i < _allSlots.Count; i++)
        {
            if (i < itemCount)
            {
                var data = savedItems[i];
                var staticInfo = configTable.GetItemByID(data.id);
                _allSlots[i].SetItem(staticInfo.image, data.num, data.isNew, data.uid, staticInfo.name);
            }
            else if (i < targetTotal)
            {

                _allSlots[i].SetEmpty();
            }
            else
            {

                _allSlots[i].Hide();
            }
        }
    }

    private void AdjustSlotCount(int targetCount)
    {
        while (_allSlots.Count < targetCount)
        {
            GameObject go = Instantiate(slotPrefab, contentParent);
            _allSlots.Add(go.GetComponent<SlotUI>());
        }
    }
}