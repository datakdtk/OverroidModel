using System.Collections.Generic;

namespace OverroidModel.GameAction.Commands
{
    /// <summary>
    /// Interface of actions that require player's choice.
    /// </summary>
    public interface IGameCommand : IGameAction
    {

        /// <summary>
        /// Player who chooses action's details.
        /// </summary>
        public PlayerAccount CommandingPlayer { get; }

        /// <summary>
        /// Parameters that are required for restoring other than CommandingPlayer.
        /// </summary>
        public Dictionary<string, string> ParametersToSave { get; }

        /// <summary>
        /// Checks if the command's detail is applicable to the game.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when the command is inapplicable.</exception>
        internal void Validate(IGameInformation g);

    }
}
