public class MonoHelperSystem : SingletonBase
{
    #region Singleton
    public static MonoHelperSystem Instance { get; private set; }

    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}