using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPanel : MonoBehaviour
{
    public List<PackageLocalItem> chestItems = new List<PackageLocalItem>();
    private static string _savePath => Application.persistentDataPath + "/Chest1.json";

    private void Start()
    {

        PackageLocalData.Instance.Save(_savePath);
    }
}
