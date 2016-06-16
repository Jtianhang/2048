//          MyEditorTool.cs
using UnityEngine;
using UnityEditor;
using System;

public class MyEditorTool : ScriptableObject
{
	//  设置菜单Tool 下的 MyTool 下的 Enable\Disable Multi GameObj 快捷键为  command 加shift 加 d  <MAC上的>
	public const string MENU_DISABLE_SELECTED_GAMEOBJ = "Tool/MyTool/Enable\\Disable Multi GameObj %#d";   //%#d 即代表 command 加shift 加 d快捷键

	[MenuItem(MENU_DISABLE_SELECTED_GAMEOBJ, true)]
	static bool ValidateSelectEnableODisable()
	{
		GameObject[] gobj = GetSelectedGameObject() as GameObject[];
		if (gobj == null)
		{
			return false;
		}
		if (gobj.Length == 0)
		{
			return false;
		}
		return true;

	}

	[MenuItem(MENU_DISABLE_SELECTED_GAMEOBJ)]
	static void SelectEnableODisable()
	{
		GameObject[] gobj = GetSelectedGameObject() as GameObject[];
		bool enable = !gobj[0].activeSelf;
		foreach (GameObject go in gobj)
		{
			EnableODisableChildNote(go.transform, enable);
		}
	}
	//激活或者关闭选中的物体及其子物体
	public static void EnableODisableChildNote(Transform parent, bool enable)
	{
		parent.gameObject.SetActive(enable);
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (child.childCount != 0)
			{
				EnableODisableChildNote(child, enable);
			}
			else
			{
				child.gameObject.SetActive(enable);
			}
		}
	}
	// 返回选中的物体
	static GameObject[] GetSelectedGameObject()
	{
		return Selection.gameObjects;
	}
}