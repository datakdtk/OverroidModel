using OverroidModel.Exceptions;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game
{
    class CommandAuthorizer<T> : ICommandAuthorizer where T : IGameAction
    {
        readonly PlayerAccount expectedPlayer;

        public CommandAuthorizer(PlayerAccount expectedPlayer)
        {
            this.expectedPlayer = expectedPlayer;
        }

        /// <inheritdoc cref="ICommandAuthorizer.Authorize(IGameAction)"/>
        public void Authorize(IGameCommand command)
        {
            if (!(command is T))
            { 
                throw new UnavailableActionException(
                    string.Format("unexpected action class. expected: {0}, given: {1}", nameof(T), command.GetType())
                );
            }
            if (!command.CommandingPlayer.Equals(this.expectedPlayer))
            {
                throw new UnavailableActionException(
                    string.Format("command by unexpected player. expected: {0}, given: {1}", this.expectedPlayer.ID, command.CommandingPlayer.ID)
                );
            }
        }
    }
}
