using System;
using System.Collections.Generic;
using System.Text;
using OverroidModel.Game.Actions;

namespace OverroidModel.Game
{
    public interface IMutableGame : IGameInformation
    {
        /// <summary>
        /// Create new battle and add it to the game.
        /// </summary>
        internal void AddNewRound();

        /// <summary>
        /// Be ready to accept game command.
        /// </summary>
        /// <param name="a">Object to check commands is applicable.</param>
        internal void AddCommandAuthorizer(ICommandAuthorizer a);

        /// <summary>
        /// Disable card effects of given player and given battle round.
        /// </summary>
        /// <param name="targetPlayer">Player whose card effect is to be disabled.</param>
        /// <param name="round">Disabled battle round.</param>
        internal void DisableRoundEffects(PlayerAccount targetPlayer, ushort round);

        /// <summary>
        /// Reserve a game action to be resolved.
        /// The first reserved action will be resolved last (resolved in stack-order).
        /// </summary>
        internal void PushToActionStack(IGameAction a);

        /// <summary>
        /// Set a Payer who wins by a card effect.
        /// </summary>
        /// <param name="p">Player to win.</param>
        internal void SetSpecialWinner(PlayerAccount p);

    }
}
