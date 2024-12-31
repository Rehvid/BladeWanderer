namespace RehvidGames.AI.States.Interfaces
{
    public interface IState
    {
        public void Enter();
        public void Execute();
        public void Exit();
        public IState GetNextState();
    }
}