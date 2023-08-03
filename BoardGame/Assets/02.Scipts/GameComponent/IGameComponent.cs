namespace BoardGame.Util
{
    public interface IGameComponent
    {
        void UpdateState(GameState state);

        void OnDisable();
    }
}