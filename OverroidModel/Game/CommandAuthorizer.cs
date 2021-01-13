﻿using OverroidModel.Exceptions;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game
{
    /// <summary>
    /// Implementation of ICommandAuthorizer
    /// </summary>
    /// <typeparam name="T">GameCommand class that expected to be passed to the game.</typeparam>
    class CommandAuthorizer<T> : ICommandAuthorizer where T : IGameCommand
    {
        readonly PlayerAccount expectedPlayer;

        /// <param name="expectedPlayer">Player expected to command.</param>
        public CommandAuthorizer(PlayerAccount expectedPlayer)
        {
            this.expectedPlayer = expectedPlayer;
        }

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