namespace BoardGame.Util
{
    public abstract class GameComponent : IGameComponent
    {
        GameManager game;

        protected GameComponent(GameManager game)
        {
            this.game = game;
        }

        public virtual void UpdateState(GameState state)
        {
            switch (state)
            {
                case GameState.Init:
                    Initialize();
                    break;
                case GameState.Setting:
                    OnSetting();
                    break;
                case GameState.Main:
                    OnRunning();
                    break;
                case GameState.Move:
                    OnMove();
                    break;
                case GameState.TakeOver:
                    OnTakeOver();
                    break;
                case GameState.Select:
                    OnSelect();
                    break;
                case GameState.Build:
                    OnBuild();
                    break;
                case GameState.Over:
                    OnOver();
                    break;
            }
        }

        protected virtual void Initialize() { }

        protected virtual void OnSetting() { }

        protected virtual void OnRunning() { }

        protected virtual void OnUpdate() { }

        protected virtual void OnMove() { }

        protected virtual void OnTakeOver() { }

        protected virtual void OnSelect() { }

        protected virtual void OnBuild() { }

        protected virtual void OnOver() { }

        public void OnDisable() { }

        public void OnRoutine()
        {
            OnUpdate();
        }
    }
}
