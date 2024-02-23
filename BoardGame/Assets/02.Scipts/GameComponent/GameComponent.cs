using UnityEngine;

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
                case GameState.Standby:
                    OnStandby();
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
                case GameState.Over:
                    OnOver();
                    break;
            }
        }

        protected virtual void Initialize() { }

        protected virtual void OnStandby() { }

        protected virtual void OnSetting() { }

        protected virtual void OnRunning() { }

        protected virtual void OnUpdate() { }

        protected virtual void OnMove() { }

        protected virtual void OnOver() { }

        public void OnDisable() { }

        public void OnRoutine()
        {
            OnUpdate();
        }
    }
}
