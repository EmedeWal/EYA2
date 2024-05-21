public class DeathUI : FadingUI
{
    private void OnEnable()
    {
        PlayerHealth.PlayerDeath += DeathUI_PlayerDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.PlayerDeath -= DeathUI_PlayerDeath;
    }

    private void DeathUI_PlayerDeath()
    {
        DisplayMessage();
    }
}
