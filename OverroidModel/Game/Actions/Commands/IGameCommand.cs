namespace OverroidModel.Game.Actions.Commands
{
    public interface IGameCommand : IGameAction
    {
        PlayerAccount CommandingPlayer { get; }
    }
}
