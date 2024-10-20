namespace RehvidGames.Interfaces
{
    public interface IAIState
    {
        public void Enter();
        public void Execute();
        public bool IsActive();
        public void Exit();
        public IAIState GetNextState();
    }
}