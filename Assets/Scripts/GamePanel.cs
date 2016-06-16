using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePanel : MonoBehaviour
{
	public static GamePanel Instance;
	public GameObject rootBg;
	public Transform prefabNum;
	public UILabel currentScoreLabel;
	public UILabel bestScoreLabel;
	public AudioSource audioButton;
	public AudioSource audioCombine;
	private Nums[,] _numArray = new Nums[4, 4];
	private int _currentScore;
	private int _bestScore;
	void Awake()
	{
		Instance = this;
	}
	void OnDestroy()
	{
		Nums.IsMove = true;
		Instance = null;
	}
	void Start()
	{
		_bestScore = PlayerPrefs.GetInt("BestScore", 0);
		bestScoreLabel.text = _bestScore.ToString();
		CreateNum();
	}
	/// <summary>
	/// 随机生成数字
	/// </summary>
	void CreateNum()
	{
		int posXIndex = 0;
		int posYIndex = 0;
		do
		{
			posXIndex = Random.Range(0, 4);
			posYIndex = Random.Range(0, 4);
		} while (_numArray[posXIndex, posYIndex] != null);
		uint num = 0;
		if (Random.value < 0.8f)
		{
			num = 2;
		}
		else
		{
			num = 4;
		}
		GameObject prefab = Instantiate(prefabNum.gameObject);
		prefab.transform.parent = rootBg.transform;
		prefab.transform.localScale = Vector3.one;
		prefab.transform.localPosition = new Vector3(-165 + posXIndex * 110, -165 + posYIndex * 110);
		prefab.GetComponent<Nums>().SetNums(num);
		_numArray[posXIndex, posYIndex] = prefab.GetComponent<Nums>();
		audioCombine.Play();
	}

	void AddCurrentSocre(uint score)
	{
		_currentScore += (int) score;
		currentScoreLabel.text = _currentScore.ToString();
		if (_currentScore > _bestScore)
		{
			_bestScore = _currentScore;
			bestScoreLabel.text = _bestScore.ToString();
		}
	}

	public int GetBestScore()
	{
		return (int)_bestScore;
	}
	bool IsGameOver()
	{
		int numsCount = 0;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (_numArray[i, j] == null)
				{
					numsCount++;
				}
				else if (_numArray[i, j].NumValue == 2048)
				{
					MessageBox.Instance.ShowInfo("2048！");
				}
			}
		}
		//首先保证可以有位置可以生产数字
		if (numsCount == 0)
		{
			Debug.Log("GameOver!");
			Nums.IsMove = false;
			MessageBox.Instance.ShowInfo("游戏结束！");
			return true;
		}
		return false;
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			MessageBox.Instance.ShowInfo("是否返回菜单？");
		}
		if (Nums.IsMove)
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				MoveUp();
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				MoveDown();
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				MoveLeft();
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				MoveRight();
			}
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
				float Movelength = 30f;
				if (Mathf.Abs(touchDeltaPos.x) > Mathf.Abs(touchDeltaPos.y))
				{
					if (touchDeltaPos.x > Movelength * 0.5)
					{
						MoveRight();
					}
					else if (touchDeltaPos.x < -Movelength * 0.5)
					{
						MoveLeft();
					}
				}
				else
				{
                    if (touchDeltaPos.y > Movelength * 0.5)
					{
						MoveUp();
					}
                    else if (touchDeltaPos.y < -Movelength * 0.5)
					{
						MoveDown();
					}
				}
			}
		}
	}

	void MoveUp()
	{
		bool isAction = false;
		for (int x = 0; x < 4; x++)
		{
			bool isHasDestroy = true;
			for (int y = 2; y >= 0; y--)
			{
				for (int z = y; z <= 2; z++)
				{
					if (_numArray[x, z] != null)
					{
						if (_numArray[x, z + 1] == null)
						{
							isAction = true;
							_numArray[x, z].SetPositionAndPlay(x, z + 1);
							_numArray[x, z + 1] = _numArray[x, z];
							_numArray[x, z] = null;
						}
						else if (isHasDestroy && _numArray[x, z].NumValue == _numArray[x, z + 1].NumValue)
						{
							isAction = true;
							isHasDestroy = false;
							_numArray[x, z + 1].SetPositionAndPlay(x, z + 1, true);
							_numArray[x, z].SetPositionAndPlay(x, z + 1);
							AddCurrentSocre(_numArray[x, z].NumValue);
							_numArray[x, z].SetNums(_numArray[x, z].NumValue * 2);
							_numArray[x, z + 1] = _numArray[x, z];
							_numArray[x, z] = null;
							audioButton.Play();
						}
					}
				}
			}
		}
		if (!IsGameOver() && isAction)
		{
			CreateNum();
		}
	}
	void MoveDown()
	{
		bool isAction = false;
		for (int x = 0; x < 4; x++)
		{
			bool isHasDestroy = true;
			for (int y = 1; y <= 3; y++)
			{
				for (int z = y; z >= 1; z--)
				{
					if (_numArray[x, z] != null)
					{
						if (_numArray[x, z - 1] == null)
						{
							isAction = true;
							_numArray[x, z].SetPositionAndPlay(x, z - 1);
							_numArray[x, z - 1] = _numArray[x, z];
							_numArray[x, z] = null;
						}
						else if (isHasDestroy && _numArray[x, z].NumValue == _numArray[x, z - 1].NumValue)
						{
							isAction = true;
							isHasDestroy = false;
							_numArray[x, z - 1].SetPositionAndPlay(x, z - 1, true);
							_numArray[x, z].SetPositionAndPlay(x, z - 1);
							AddCurrentSocre(_numArray[x, z].NumValue);
							_numArray[x, z].SetNums(_numArray[x, z].NumValue * 2);
							_numArray[x, z - 1] = _numArray[x, z];
							_numArray[x, z] = null;
							audioButton.Play();
						}
					}
				}
			}
		}
		if (!IsGameOver() && isAction)
		{
			CreateNum();
		}
	}

	void MoveLeft()
	{
		bool isAction = false;
		for (int y = 0; y < 4; y++)
		{
			bool isHasDestroy = true;
			for (int x = 1; x <= 3; x++)
			{
				for (int z = x; z >= 1; z--)
				{
					if (_numArray[z, y] != null)
					{
						if (_numArray[z - 1, y] == null)
						{
							isAction = true;
							_numArray[z, y].SetPositionAndPlay(z - 1, y);
							_numArray[z - 1, y] = _numArray[z, y];
							_numArray[z, y] = null;
						}
						else if (isHasDestroy && _numArray[z, y].NumValue == _numArray[z - 1, y].NumValue)
						{
							isAction = true;
							isHasDestroy = false;
							_numArray[z - 1, y].SetPositionAndPlay(z - 1, y, true);
							_numArray[z, y].SetPositionAndPlay(z - 1, y);
							AddCurrentSocre(_numArray[z, y].NumValue);
							_numArray[z, y].SetNums(_numArray[z, y].NumValue * 2);
							_numArray[z - 1, y] = _numArray[z, y];
							_numArray[z, y] = null;
							audioButton.Play();
						}
					}
				}

			}
		}
		if (!IsGameOver() && isAction)
		{
			CreateNum();
		}
	}
	void MoveRight()
	{
		bool isAction = false;
		for (int y = 0; y < 4; y++)
		{
			bool isHasDestroy = true;
			for (int x = 2; x >= 0; x--)
			{
				if (_numArray[x, y] != null)
				{
					for (int z = x; z <= 2; z++)
					{
						if (_numArray[z + 1, y] == null)
						{
							isAction = true;
							_numArray[z, y].SetPositionAndPlay(z + 1, y);
							_numArray[z + 1, y] = _numArray[z, y];
							_numArray[z, y] = null;
						}
						else if (isHasDestroy && _numArray[z, y].NumValue == _numArray[z + 1, y].NumValue)
						{
							isAction = true;
							isHasDestroy = false;
							_numArray[z + 1, y].SetPositionAndPlay(z + 1, y, true);
							_numArray[z, y].SetPositionAndPlay(z + 1, y);
							AddCurrentSocre(_numArray[z, y].NumValue);
							_numArray[z, y].SetNums(_numArray[z, y].NumValue * 2);
							_numArray[z + 1, y] = _numArray[z, y];
							_numArray[z, y] = null;
							audioButton.Play();
						}
					}
				}
			}
		}
		if (!IsGameOver() && isAction)
		{
			CreateNum();
		}
	}
}