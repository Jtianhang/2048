using UnityEngine;
using System.Collections;

public class Nums : MonoBehaviour
{
	private static bool _isMove = true;

	/// <summary>
	/// 当前是否在移动中
	/// </summary>
	public static bool IsMove
	{
		get { return _isMove; }
		set { _isMove = value; }
	}
	private uint _numValue = 0;
	/// <summary>
	/// 当前num的值
	/// </summary>
	public uint NumValue
	{
		get { return _numValue; }
		private set { _numValue = value; }
	}

	private bool _isDestroy = false;
	public UISprite numSprite;
	public TweenPosition tweenPos;

	public void SetNums(uint num)
	{
		this.name = num.ToString();
		_numValue = num;
		numSprite.spriteName = num.ToString();
	}
	public void SetPositionAndPlay(int posX, int posY, bool isDestroy = false)
	{
		_isDestroy = isDestroy;
		IsMove = false;
		tweenPos.from = this.gameObject.transform.localPosition;
		tweenPos.to = new Vector3(-165 + posX * 110, -165 + posY * 110);
		tweenPos.ResetToBeginning();
		tweenPos.PlayForward();
	}
	public void OnEndOfPosMove()
	{
		IsMove = true;
		if (_isDestroy)
		{
			DestroyThisGameObject(this.gameObject);
		}
	}

	static void DestroyThisGameObject(GameObject obj)
	{
		DestroyObject(obj);
	}
}
