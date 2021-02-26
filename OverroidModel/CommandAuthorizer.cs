using System;
using OverroidModel.Exceptions;
using OverroidModel.GameAction.Commands;

namespace OverroidModel
{
    /// <summary>
    /// Interface to external game command is applicable for current game state.
    /// </summary>
    interface ICommandAuthorizer
    {

        /// <summary>
        /// Indicate which type of GameCommand class and which commanding player is expected to be given.
        /// </summary>
        public (Type type, PlayerAccount player) RequiredCommandInfo { get; }

        /// <summary>
        /// What kind of GameCommand is expected to be given;
        /// </summary>
        public ICommandRequirement CommandRequirement { get; }

        /// <summary>
        /// Checks if given command is acceptable for current game state.
        /// </summary>
        /// <param name="command"> Command from a player.</param>
        /// <exception cref="UnavailableActionException"> Thrown if given command is not acceptable.</exception>
        public void Authorize(IGameCommand command);

    }

    /// <summary>
    /// Implementation of ICommandAuthorizer
    /// </summary>
    /// <typeparam name="T">GameCommand class that expected to be passed to the game.</typeparam>
    class CommandAuthorizerImplement<T> : ICommandAuthorizer where T : IGameCommand
    {
        readonly PlayerAccount expectedPlayer;
        readonly CommandRequirementImplement<T> requirement;

        /// <param name="expectedPlayer">Player expected to command.</param>
        public CommandAuthorizerImplement(PlayerAccount expectedPlayer)
        {
            this.expectedPlayer = expectedPlayer;
            this.requirement = new CommandRequirementImplement<T>(expectedPlayer);
        }

        public (Type type, PlayerAccount player) RequiredCommandInfo => (typeof(T), expectedPlayer);

        public ICommandRequirement CommandRequirement => requirement;


        public void Authorize(IGameCommand command)
        {
            if (!(command is T))
            {
                throw new UnavailableActionException(
                    string.Format("unexpected action class. expected: {0}, given: {1}", nameof(T), command.GetType())
                );
            }
            if (!command.CommandingPlayer.Equals(expectedPlayer))
            {
                throw new UnavailableActionException(
                    string.Format("command by unexpected player. expected: {0}, given: {1}", expectedPlayer.ID, command.CommandingPlayer.ID)
                );
            }
        }
    }
}
