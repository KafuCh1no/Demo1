using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;
using Cinemachine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private TextAsset dialogData;
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionsGroup;

    [SerializeField] private CinemachineVirtualCamera dialogCam;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    private string[] rowDialogs;
    private int currentIndex;
    private bool isSelecting;
    private Transform camPivot;

    // 当对话时再设为true，不然一直运行update函数
    private bool isTalking;


    private void Awake()
    {
        Instance = this;
        isSelecting = false;
        currentIndex = 0;
        isTalking = false;
    }

    public void UpdateDialog(TextAsset _textAsset, Transform _transform)
    {
        rowDialogs = _textAsset.text.Split('\n');
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UpdateText(0);
        dialogUI.SetActive(true);
        isTalking = true;

        camPivot = _transform.Find("CamPivot");
        targetGroup.AddMember(camPivot, 1f, 0.5f);
        dialogCam.Priority = 20;

    }

    private void Update()
    {
        if (!isTalking || isSelecting)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            UpdateText(currentIndex);
        }
    }

    public void UpdateText(int Index)
    {
        Debug.Log($"{Index}");
        for(int i = 0; i < rowDialogs.Length; i++)
        {
            string[] cells = rowDialogs[i].Split(',');

            
            if (cells[0] == "&" && int.Parse(cells[1]) == Index)
            {
                isSelecting = false;
                nameText.text = cells[2];
                contentText.text = cells[3];
                currentIndex = int.Parse(cells[4]);

                foreach (Transform child in optionsGroup)
                {
                    Destroy(child.gameObject);
                }

                break;
            }
            else if(cells[0] == "|" && int.Parse(cells[1]) == Index)
            {
                isSelecting = true;
                UpdateOptions(i);
                break;
            }else if (cells[0] == "end")
            {
                // 对话结束
                dialogUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isTalking = false;
                dialogCam.Priority = 5;
                targetGroup.RemoveMember(camPivot);
            }
        }
    }

    public void UpdateOptions(int _index)
    {
        string[] cells = rowDialogs[_index].Split(',');
        int targetIndex = int.Parse(cells[4]);
        if (cells[0] != "|")
        {
            return;
        }
        GameObject button = Instantiate(optionPrefab, optionsGroup);
        button.GetComponentInChildren<TMP_Text>().text = cells[3];
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("已点击");
            UpdateText(targetIndex);
        });
        UpdateOptions(_index + 1);
    }
}
