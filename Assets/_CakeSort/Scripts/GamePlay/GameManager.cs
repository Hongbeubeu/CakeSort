using Ultimate.Core.Runtime.Singleton;

public class GameManager : Singleton<GameManager>
{
    public ObjectPooler ObjectPooler;
    public GameConfig GameConfig;
    public GameSettings GameSettings;

    public override void Init()
    {
    }
}