using System.Collections;
using Controller;
using Source.Scripts.Windows;
using UnityEngine;

/// <summary>
/// This class store user game date
/// </summary>
public class GameState
{
	#region Singleton
	private static GameState _instance;
	public static GameState Instance
	{
		get
		{
			if (_instance != null)
				return _instance;

			_instance = new GameState();

			return _instance != null ? _instance : null;
		}
	}
	
	private GameState()
	{
		InitGameState();
	}
	#endregion
	
	/// <summary>
	/// User Game Name
	/// </summary>
	public string UserName { get; private set; }
	
	/// <summary>
	/// User Prestige Count
	/// </summary>
	public int Prestige { get; private set; }
	
	/// <summary>
	/// User Game Id
	/// </summary>
	public string GameId { get; private set; }
	
	/// <summary>
	/// User completed quests
	/// </summary>
	public ArrayList CompleteQuests { get; private set; }
	
	/// <summary>
	/// User Game Coins Count
	/// </summary>
	public int Coins { get; private set; }

	/// <summary>
	/// Player tutorials states
	/// </summary>
	private ArrayList _tutorials;

	/// <summary>
	/// Table with user data 
	/// </summary>
	private Hashtable _gameState;

	/// <summary>
	/// Initialize user data from player prefs
	/// </summary>
	private void InitGameState()
	{
		var gameStateStr = PlayerPrefs.GetString("GameState", string.Empty);

		_gameState = string.IsNullOrEmpty(gameStateStr) ? new Hashtable() : JSON.JsonDecode(gameStateStr);

		if (!_gameState.ContainsKey("UserName"))
		{
			_gameState.Add("UserName", "User");
		}
		
		if (!_gameState.ContainsKey("GameStateId"))
		{
			var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			var curTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
			_gameState.Add("GameStateId", string.Format("{0}_{1}", System.Guid.NewGuid(), curTime));
		}
		
		if (!_gameState.ContainsKey("Prestige"))
		{
			_gameState.Add("Prestige", 0.0f);
		}
		
		if (!_gameState.ContainsKey("Coins"))
		{
			_gameState.Add("Coins", 0.0f);
		}

		if (!_gameState.ContainsKey("CompleteQuests"))
		{
			_gameState.Add("CompleteQuests", new ArrayList());
		}
		
		if (!_gameState.ContainsKey("Tutorials"))
		{
			_gameState.Add("Tutorials", new ArrayList());
		}

		UserName = (string)_gameState["UserName"];
		Prestige = (int)((float)_gameState["Prestige"]);
		Coins = (int)((float)_gameState["Coins"]);
		GameId = (string)_gameState["GameStateId"];
		CompleteQuests = (ArrayList)_gameState["CompleteQuests"];
		
		_tutorials = (ArrayList)_gameState["Tutorials"];
	}

	/// <summary>
	/// Set new game coins value
	/// </summary>
	/// <param name="coins"></param>
	public void SetNewCoinsValue(int coins)
	{
		_gameState["Coins"] = (float)coins;
		Coins = coins;
		Save();
	}
	
	/// <summary>
	/// Set new user name
	/// </summary>
	/// <param name="name"></param>
	public void SetNewNameValue(string name)
	{
		_gameState["UserName"] = name;
		UserName = name;
		Save();
	}

	/// <summary>
	/// Added complete quest in game state progress
	/// </summary>
	/// <param name="key"></param>
	public void AddCompleteQuest(string key)
	{
		CompleteQuests.Add(key);
		_gameState["CompleteQuests"] = CompleteQuests;
		Save();
	}
	
	/// <summary>
	/// Add player prestige count
	/// </summary>
	/// <param name="points"></param>
	public void AddPrestige(int points)
	{
		Prestige = Prestige + points;
		_gameState["Prestige"] = Prestige;
		Save();
	}
	
	/// <summary>
	/// Set prestige new value
	/// </summary>
	/// <param name="points"></param>
	public void SetPrestige(int points)
	{
		Prestige = points;
		_gameState["Prestige"] = Prestige;
		Save();
	}

	/// <summary>
	/// Return true if quest completed
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public bool IsCompleteQuest(string key)
	{
		return CompleteQuests.Contains(key);
	}
	
	/// <summary>
	/// Save user data in player prefs
	/// </summary>
	public void Save()
	{
		_gameState["CompleteQuests"] = CompleteQuests.ToArray();
		var text = JSON.JsonEncode(_gameState);
		PlayerPrefs.SetString("GameState", text);
	}

	public void Clear()
	{
		PlayerPrefs.SetString("GameState", string.Empty);
	}

	public bool IsTutorialComplete(string id)
	{
		return _tutorials.Contains(id);
	}

	public void CompleteTutorial(string id)
	{
		_tutorials.Add(id);
		_gameState["Tutorials"] = _tutorials;
		Save();
	}
}
