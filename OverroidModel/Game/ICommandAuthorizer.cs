using OverroidModel.Exceptions;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game
{
    /// <summary>
    /// Interface to external game command is applicable for current game state.
    /// </summary>
    interface ICommandAuthorizer
    {
        /// <summary>
        /// Checks if given command is acceptable for current game state.
        /// </summary>
        /// <param name="command"> Command from a player.</param>
        /// <exception cref="UnavailableActionException"> Thrown if given command is not acceptable.</exception>
        public void Authorize(IGameCommand command);
    }
}
