using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;
using System.Collections;
using OfficeOpenXml;
using System;
using System.IO;
using System.Collections.Generic;

public class MapDataTableReader : EditorWindow
{
	public UnityEngine.Object ProtoExcelFile = null;
	private static string lastMsg = string.Empty;
	private string selectedExcelPath = "Assets/StarryNight/Tables/MapDataTable.xlsx";
	private string selectedExportPath = "Assets/JoyconFrameWork/Resources/DataTables";
    [MenuItem ("DataReader/MapDataTable Reader", false, 1)]

	public static void Init(){
		MapDataTableReader window = (MapDataTableReader)EditorWindow.GetWindow (typeof (MapDataTableReader));
	}

	void OnGUI () {
		string filePath = string.Empty;
		string tempPath = string.Empty;

		if (selectedExcelPath != string.Empty)
		{
			filePath = Path.GetFullPath(selectedExcelPath);
		} else {
			ProtoExcelFile = EditorGUILayout.ObjectField(ProtoExcelFile, typeof(UnityEngine.Object), false);
			if( ProtoExcelFile != null){
				filePath = Path.GetFullPath(AssetDatabase.GetAssetPath(ProtoExcelFile));
			}
		}

		if (filePath != string.Empty)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("ExcelFile loc : "  + filePath);
			tempPath = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				tempPath = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(tempPath))
				{
					tempPath = Path.GetDirectoryName(tempPath);
				}
				break;
			}
			if(GUILayout.Button("Open Excel File", GUILayout.Height (60)))
			{
				System.Diagnostics.Process.Start(filePath);
			}

			GUILayout.EndHorizontal();

			if( filePath != string.Empty)
			{
				if(GUILayout.Button("Import MapDataTable from Excel", GUILayout.Height (100)))
				{
					ReadExcelFile(filePath, selectedExportPath);
				}
			}
		}
		if (lastMsg != string.Empty)
		{
			GUILayout.Label(lastMsg);
		}
	}

	static bool ReadExcelFile (string excelPath, string prefabPath) {
		lastMsg = string.Empty;

		try
		{
			Excel itemData =  ExcelHelper.LoadExcel(excelPath);

			if (itemData == null)
			{
				return false;
			}

			List<ExcelTable> itemDataTable = itemData.Tables;

			string prefabFilePath = prefabPath + "/MapDataTable.asset";
			ScriptableObject mapTableSO = (ScriptableObject)AssetDatabase.LoadAssetAtPath(prefabFilePath,typeof(ScriptableObject));
			if(mapTableSO == null)
			{
                mapTableSO = ScriptableObject.CreateInstance<MapDataTable>();
                AssetDatabase.CreateAsset(mapTableSO, prefabFilePath);
                AssetImporter.GetAtPath(prefabFilePath).SetAssetBundleNameAndVariant("datatables", "");
                mapTableSO.hideFlags = HideFlags.NotEditable;
            }

            MapDataTable mapDataTable = (MapDataTable)mapTableSO;

            mapDataTable.mapDataList.Clear();

            if (itemDataTable.Count > 0)
            {
                for (int i = 0; i < itemDataTable.Count; i++)
                {
                    if (itemDataTable[i].TableName == "Sheet1")
                    {
                        int indexColumn = 0;
                        int tagColumn = 0;
                        int levelColumn = 0;
                        int widthColumn = 0;
                        int heightColumn = 0;

						for (int column = 1; column <= itemDataTable[i].NumberOfColumns; column++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Index")
                                indexColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Tag")
                                tagColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Level")
                                levelColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Width")
                                widthColumn = column;
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Height")
								heightColumn = column;
						}

                        for (int row = 2; row <= itemDataTable[i].NumberOfRows; row++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(row, indexColumn)).Equals(""))
                                continue;

                            int index = Convert.ToInt32(itemDataTable[i].GetValue(row, indexColumn));
                            string tag = Convert.ToString(itemDataTable[i].GetValue(row, tagColumn));
                            int level = Convert.ToInt32(itemDataTable[i].GetValue(row, levelColumn));
                            float width = Convert.ToSingle(itemDataTable[i].GetValue(row, widthColumn));
                            float height = Convert.ToSingle(itemDataTable[i].GetValue(row, heightColumn));

							MapData mapData = new MapData();
                            mapData.index = index;
                            mapData.tag = tag;
                            mapData.level = level;
                            mapData.width = width;
                            mapData.height = height;

							mapDataTable.mapDataList.Add(mapData);
                        }
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.SetDirty(mapTableSO);
                lastMsg = "Succeeded import data to prefab file : " + prefabFilePath;
            }
            else
            {
                lastMsg = "Result : Fail. Reason : Data was not found.";
            }

            return true;
		} catch (Exception e)
		{
			UnityEngine.Debug.Log(e.Message);
			lastMsg = "Result : fail\nMessage : " + e.Message;
			return false;
		} finally {
		}
	}
}
