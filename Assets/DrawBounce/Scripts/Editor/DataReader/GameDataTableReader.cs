using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;
using System.Collections;
using OfficeOpenXml;
using System;
using System.IO;
using System.Collections.Generic;

public class GameDataTableReader : EditorWindow
{
	public UnityEngine.Object ProtoExcelFile = null;
	private static string lastMsg = string.Empty;
	private string selectedExcelPath = "Assets/DrawBounce/Tables/GameDataTable.xlsx";
	private string selectedExportPath = "Assets/Resources/DataTables";
    [MenuItem ("DataReader/GameDataTable Reader", false, 1)]

	public static void Init(){
		GameDataTableReader window = (GameDataTableReader)EditorWindow.GetWindow (typeof (GameDataTableReader));
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
				if(GUILayout.Button("Import GameDataTable from Excel", GUILayout.Height (100)))
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

			string prefabFilePath = prefabPath + "/GameDataTable.asset";
			ScriptableObject mapTableSO = (ScriptableObject)AssetDatabase.LoadAssetAtPath(prefabFilePath,typeof(ScriptableObject));
			if(mapTableSO == null)
			{
                mapTableSO = ScriptableObject.CreateInstance<GameDataTable>();
                AssetDatabase.CreateAsset(mapTableSO, prefabFilePath);
                AssetImporter.GetAtPath(prefabFilePath).SetAssetBundleNameAndVariant("datatables", "");
                mapTableSO.hideFlags = HideFlags.NotEditable;
            }

            GameDataTable gameDataTable = (GameDataTable)mapTableSO;

            gameDataTable.gameLevelInfoList.Clear();
            gameDataTable.targetHeightList.Clear();
			gameDataTable.shopInfoList.Clear();

			if (itemDataTable.Count > 0)
            {
                for (int i = 0; i < itemDataTable.Count; i++)
                {
                    if (itemDataTable[i].TableName == "GameLevel")
					{
						int indexColumn = 0;
						int levelColumn = 0;
						int mapNumberColumn = 0;

						for (int column = 1; column <= itemDataTable[i].NumberOfColumns; column++)
						{
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Index")
								indexColumn = column;
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Level")
								levelColumn = column;
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "MapNumber")
								mapNumberColumn = column;
						}

						for (int row = 2; row <= itemDataTable[i].NumberOfRows; row++)
						{
							if (Convert.ToString(itemDataTable[i].GetValue(row, indexColumn)).Equals(""))
								continue;
							if (Convert.ToString(itemDataTable[i].GetValue(row, levelColumn)).Equals(""))
								continue;

							int index = Convert.ToInt32(itemDataTable[i].GetValue(row, indexColumn));
							int level = Convert.ToInt32(itemDataTable[i].GetValue(row, levelColumn));
							int mapNumber = Convert.ToInt32(itemDataTable[i].GetValue(row, mapNumberColumn));

							GameLevelInfo gameLevelInfo = new GameLevelInfo();
							gameLevelInfo.index = index;
							gameLevelInfo.level = level;
							gameLevelInfo.mapNumber = mapNumber;

							gameDataTable.gameLevelInfoList.Add(gameLevelInfo);
						}
					}
                    else if (itemDataTable[i].TableName == "TargetHeight")
					{
						int indexColumn = 0;
						int levelColumn = 0;
						int heightColumn = 0;

						for (int column = 1; column <= itemDataTable[i].NumberOfColumns; column++)
						{
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Index")
								indexColumn = column;
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Level")
								levelColumn = column;
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "TargetHeight")
								heightColumn = column;
						}

						for (int row = 2; row <= itemDataTable[i].NumberOfRows; row++)
						{
							if (Convert.ToString(itemDataTable[i].GetValue(row, indexColumn)).Equals(""))
								continue;
							if (Convert.ToString(itemDataTable[i].GetValue(row, levelColumn)).Equals(""))
								continue;

							int index = Convert.ToInt32(itemDataTable[i].GetValue(row, indexColumn));
							int level = Convert.ToInt32(itemDataTable[i].GetValue(row, levelColumn));
							float targetHeight = Convert.ToSingle(itemDataTable[i].GetValue(row, heightColumn));

							TargetHeightInfo targetHeightInfo = new TargetHeightInfo();
							targetHeightInfo.index = index;
							targetHeightInfo.level = level;
							targetHeightInfo.targetHeight = targetHeight;

							gameDataTable.targetHeightList.Add(targetHeightInfo);
						}
                    }
					else if (itemDataTable[i].TableName == "Shop")
					{
						int indexColumn = 0;
						int itemTypeColumn = 0;
						int priceColumn = 0;

						for (int column = 1; column <= itemDataTable[i].NumberOfColumns; column++)
						{
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Index")
								indexColumn = column;
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "ItemType")
								itemTypeColumn = column;
							if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Price")
								priceColumn = column;
						}

						for (int row = 2; row <= itemDataTable[i].NumberOfRows; row++)
						{
							if (Convert.ToString(itemDataTable[i].GetValue(row, indexColumn)).Equals(""))
								continue;
							if (Convert.ToString(itemDataTable[i].GetValue(row, itemTypeColumn)).Equals(""))
								continue;

							int index = Convert.ToInt32(itemDataTable[i].GetValue(row, indexColumn));
							string itemTypeName = Convert.ToString(itemDataTable[i].GetValue(row, itemTypeColumn));
							int price = Convert.ToInt32(itemDataTable[i].GetValue(row, priceColumn));

							ShopInfo shopInfo = new ShopInfo();
							shopInfo.index = index;
							shopInfo.itemType = ParseEnum<ShopItemType>(itemTypeName, ShopItemType.HP);
							shopInfo.price = price;

							gameDataTable.shopInfoList.Add(shopInfo);
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

	public static T ParseEnum<T>(string value, T defaultValue) where T : struct
	{
		try
		{
			T enumValue;
			if (!Enum.TryParse(value, true, out enumValue))
			{
				return defaultValue;
			}
			return enumValue;
		}
		catch (Exception)
		{
			return defaultValue;
		}
	}
}
