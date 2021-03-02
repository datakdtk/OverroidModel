using System;
using OverroidModel.GameAction.Commands;

namespace OverroidModel
{
    public interface ICommandRequirement
    {
        /// <summary>
        /// Player who publish the command.
        /// </summary>
        PlayerAccount ComandingPlayer { get; }

        /// <summary>
        /// Expected IGameCommand implementation class.
        /// </summary>
        Type CommandType { get; }

    }

    public class CommandRequirementImplement<T> : ICommandRequirement where T : IGameCommand
    {
        readonly PlayerAccount commandingPlayer;

        internal CommandRequirementImplement(PlayerAccount commandingPlayer)
        {
            this.commandingPlayer = commandingPlayer;
        }

        public PlayerAccount ComandingPlayer => commandingPlayer;

        public Type CommandType => typeof(T);
    }
}
