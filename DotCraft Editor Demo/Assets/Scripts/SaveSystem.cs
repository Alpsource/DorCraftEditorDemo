using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{

    public static void SaveToJson(List<GridElement> list)
    {
        string data = JsonUtility.ToJson(new ListContainer(list));
        Debug.Log(data);
        System.IO.File.WriteAllText(Application.dataPath + "/MatrixInfo.json", data);
    }
}
[System.Serializable]
public class GridElement
{
    public string point;
    public string color;
}
[System.Serializable]
public class ListContainer
{
    public List<GridElement> dataList;

    /// <summary>
	/// Constructor
	/// </summary>
	/// <param name="_dataList">Data list value</param>
	public ListContainer(List<GridElement> _dataList)
    {
        dataList = _dataList;
    }
}
