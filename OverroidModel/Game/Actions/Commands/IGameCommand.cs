namespace OverroidModel.Game.Actions.Commands
{
    public interface IGameCommand : IGameAction
    {
        public PlayerAccount CommandingPlayer { get; }

        internal void Validate(IGame g); 
    }
}
