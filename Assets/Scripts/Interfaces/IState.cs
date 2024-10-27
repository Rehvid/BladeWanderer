namespace RehvidGames.Interfaces
{
    public interface IState
    {
        public void Enter();
        public void Execute();
        public bool IsActive();
        public void Exit();
        public IState GetNextState();
    }
}