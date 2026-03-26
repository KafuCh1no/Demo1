using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickOptions : MonoBehaviour
{
    public static RightClickOptions Instance { get; private set; }

    //public EquipSlot _equipSlot;
    private string _currentUid;
    private CanvasGroup _canvasGroup;

    public Button deleteButton;
    public Button equipButton;

    private void Awake()
    {
        Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
        //gameObject.SetActive(false);


        deleteButton.onClick.AddListener(Delete);
        equipButton.onClick.AddListener(Equip);
    }


    public void Show(string uid, Vector2 mousePosition)
    {

        _currentUid = uid;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        transform.position = mousePosition;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void Delete()
    {
        Debug.Log($"正在尝试删除物品，传入的UID是: {_currentUid}");
        PackageLocalData.Instance.Delete(_currentUid);
        Hide();
    }

    public void Equip()
    {
        Debug.Log($"正在尝试装备物品，传入的UID是: {_currentUid}");
        PackageLocalData.Instance.Equip(_currentUid);
        EquipSlot.Instance.DisplayImage(_currentUid);
        PlayerEquipment.Instance.equipmentUpdate(_currentUid);

        Hide();
    }


    //private void OnDisable()
    //{
    //    // 当物体被隐藏时，打印出堆栈信息
    //    Debug.Log("菜单被隐藏了！来源：", this);
    //}
}
