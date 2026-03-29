using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private TextAsset dialogData;
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionsGroup;

    private string[] rowDialogs;
    private int currentIndex;
    private bool isTalking;
    private bool isSelecting;

    private void Awake()
    {
        Instance = this;
        isTalking = false;
        isSelecting = false;
        currentIndex = 0;
    }

    public void UpdateDialog(TextAsset _textAsset)
    {
        rowDialogs = _textAsset.text.Split('\n');
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isTalking = true;
        UpdateText(0);
        dialogUI.SetActive(true);
         
    }

    private void Update()
    {
        if (!isTalking || !isSelecting)
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
        foreach (var row in rowDialogs)
        {
            string[] cells = row.Split(',');
            if (cells[0] == "&" && int.Parse(cells[1]) == Index)
            {
                isSelecting = true;
                nameText.text = cells[2];
                contentText.text = cells[3];
                currentIndex = int.Parse(cells[4]);
                break;
            }
            else if(cells[0] == "|" && int.Parse(cells[1]) == Index)
            {
                Debug.Log("shengcheng");
                UpdateOptions(Index);
                isSelecting = false;
            }
        }
    }

    public void UpdateOptions(int _index)
    {
        string[] cells = rowDialogs[_index + 1].Split(',');
        Debug.Log($"{cells[0]}");
        if (cells[0] != "|")
        {
            return;
        }
        Instantiate(optionPrefab, optionsGroup);

        UpdateOptions(_index + 1);
    }
}
