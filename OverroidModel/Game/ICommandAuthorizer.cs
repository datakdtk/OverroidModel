using OverroidModel.Exceptions;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game
{
    interface ICommandAuthorizer
    {
        /// <summary>
        /// Checks if given command is acceptable for current game state.
        /// </summary>
        /// <param name="command"> Command trying to proceed game.</param>
        /// <exception cref="UnavailableActionException"> Thrown if given command is not acceptable.</exception>
        public void Authorize(IGameCommand command);
    }
}
