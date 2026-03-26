using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI itemName;
    public GameObject redDot;
    public GameObject selected;
    public GameObject options;

    private string _currentUid;
    private static SlotUI lastPointer = null;


    public void SetItem(Sprite icon, int count, bool isNew, string uid, string name)
    {
        gameObject.SetActive(true);
        iconImage.gameObject.SetActive(true);
        itemName.text = name;
        iconImage.sprite = icon;
        countText.text = count > 1 ? count.ToString() : "";
        if (redDot != null) redDot.SetActive(isNew);
        _currentUid = uid;
    }

    public void SetEmpty()
    {
        if (iconImage != null) iconImage.gameObject.SetActive(false);
        if (countText != null) countText.text = "";
        if (itemName != null) itemName.text = "";
        if (redDot != null) redDot.SetActive(false);
        _currentUid = "";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(lastPointer != null)
        {
            lastPointer.selected.SetActive(false);
        }

        if (_currentUid != "")
        {
            selected.SetActive(true);
            lastPointer = this;

            // 每个格子都是独立的实例，每个格子走代码的都不一样

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                RightClickOptions.Instance.Hide();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClickOptions.Instance.Show(_currentUid, eventData.position);
            }
        }
        else
        {
            RightClickOptions.Instance.Hide();
            selected.SetActive(false);
        }
    }

}