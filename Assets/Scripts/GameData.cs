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
    }
    
    public bool IsPaused { get; set; }
    public string LevelName { get; set; }
    public string Passenger { get; set; } // TODO replace with Passenger object?
}
