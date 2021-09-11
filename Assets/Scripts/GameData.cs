public class GameData
{
    private static GameData instance;

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }
            return instance;
        }
    }

    private GameData()
    {
        if(instance != null)
            return;
        
        Health = new Observable<int>(0);
        LevelName = new Observable<string>("");
    }

    public Observable<int> Health;
    public Observable<string> LevelName;
    public bool IsPaused { get; set; }
    public string Passenger { get; set; } // TODO replace with Passenger object?
}
