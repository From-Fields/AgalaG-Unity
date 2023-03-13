
public abstract class Singleton<T> where T: class, new()
{
    static T _instance;

    public static T Instance {
        get
        {
            if(_instance == null)
                _instance = SetInstance();
            
            return _instance;
        }
    }

    protected static T SetInstance() => new T();
}