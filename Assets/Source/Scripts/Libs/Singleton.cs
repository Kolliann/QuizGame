using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	static GameObject singleton = null;
	static T _instance = null;
	static bool applicationIsQuitting = false;

	public static T Instance
    {
		get
        {
            if (_instance == null)
            {
				if (applicationIsQuitting)
					return null;
				_instance = FindObjectOfType<T>();

                if (_instance == null)
                {
					if (singleton == null)
                    {
						singleton = GameObject.Find("Singleton");
						if (singleton == null)
							singleton = new GameObject("Singleton");
						DontDestroyOnLoad(singleton);
					}
					_instance = singleton.AddComponent<T>();
					IsInstantiated = true;
				}
			}
			return _instance;
		}
	}

	public static bool IsInstantiated = false;

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	protected virtual void OnDestroy()
    {
		applicationIsQuitting = true;
	}
}
