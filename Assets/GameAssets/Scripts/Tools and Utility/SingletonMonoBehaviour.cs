using UnityEngine;

public enum DestroyOptions
{
    DontDestroy,
    DestroyBehaviour,
    DestroyGameObject
}

/// <summary>
/// Helper class for creating singletons components in Unity.
/// Based off on this thread: <seealso cref="https://stackoverflow.com/documentation/unity3d/2137/singletons-in-unity#t=201705290349195977014"/>
/// </summary>
/// <typeparam name="T">The declaring singleton type</typeparam>
[DisallowMultipleComponent]
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static object m_globalObject = new object();
    private static bool m_hasInstance;
    private static bool m_isQuitting;
    private static volatile T m_instance;

    /// <summary>
    /// If set this singleton instance will be asigned only when needed.
    /// Default is true.
    /// </summary>
    public static bool Lazy { get; set; }

    /// <summary>
    /// Allow this singleton to search inactive objects when searching for its instance.
    /// Default is true.
    /// </summary>
    public static bool FindInactive { get; set; }

    /// <summary>
    /// Make this singleton instance persist through scenes.
    /// Default is false.
    /// </summary>
    public static bool Persist { get; set; }

    /// <summary>
    /// Destroy instances if there is more than one.
    /// Default is true.
    /// </summary>
    public static DestroyOptions DestroyOthers { get; set; }

    /// <summary>
    /// Get this singleton instance.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_isQuitting)
            {
                Debug.LogWarningFormat("Singleton '{0}' already destroyed on application quit", typeof(T).Name);
                return null;
            }

            lock (m_globalObject)
            {
                if (m_hasInstance)
                    return m_instance;

                var instances = FindInactive ? Resources.FindObjectsOfTypeAll<T>() : FindObjectsOfType<T>();

                if (instances == null || instances.Length < 1)
                {
                    Instance = new GameObject(string.Format("{0} (Singleton)", typeof(T).Name)).AddComponent<T>();
                    Debug.LogFormat(Instance, "Created singleton instance of type '{0}' {1}", typeof(T).Name, Persist ? " with DontDestoryOnLoad" : string.Empty);
                }
                else if (instances.Length >= 1)
                {
                    Instance = instances[0];

                    if (instances.Length > 1)
                    {
                        Debug.LogWarningFormat("{0} instances of singleton object '{1}'!", instances.Length, typeof(T).Name);

                        if (DestroyOthers != DestroyOptions.DontDestroy)
                            for (var i = 1; i < instances.Length; i++)
                            {
                                Debug.LogWarningFormat("Destroyed singleton instance '{0}'", typeof(T).Name);
                                if (DestroyOthers == DestroyOptions.DestroyBehaviour)
                                    Destroy(instances[i]);
                                else
                                    Destroy(instances[i].gameObject);
                            }
                    }
                }

                return m_instance;
            }
        }
        private set
        {
            m_instance = value;
            m_hasInstance = true;
            m_instance.AwakeSingleton();

            if (Persist)
                DontDestroyOnLoad(m_instance.gameObject);
        }
    }

    static SingletonMonoBehaviour()
    {
        Lazy = true;
        FindInactive = true;
        Persist = false;
        DestroyOthers = DestroyOptions.DestroyBehaviour;
    }

    /// <summary>
    /// Remember to call base.Awake() if you override this implementation.
    /// </summary>
    protected virtual void Awake()
    {
        if (Lazy)
            return;

        lock (m_globalObject)
            if (!m_hasInstance)
                Instance = (T)this;

            else if (DestroyOthers != DestroyOptions.DontDestroy && Instance != this)
            {
                Debug.LogWarningFormat("Destroyed singleton instance '{0}'", typeof(T).Name);
                if (DestroyOthers == DestroyOptions.DestroyBehaviour)
                    Destroy(this);
                else
                    Destroy(gameObject);
            }
    }

    /// <summary>
    /// This is called when one singleton instance is assigned.
    /// </summary>
    protected virtual void AwakeSingleton() { }

    /// <summary>
    /// Remember to call base.OnDestroy() if you override this implementation.
    /// </summary>
    protected virtual void OnDestroy()
    {
        m_isQuitting = true;
        m_hasInstance = false;
    }
}