﻿using System.Collections.Generic;
using System.Diagnostics;
using OverroidModel.Card;
using OverroidModel.Exceptions;

namespace OverroidModel.GameAction.Commands
{
    /// <summary>
    /// Action to place battle card from hand.
    /// </summary>
    public class CardPlacement : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName cardNameToPlace;

        /// <param name="player">Player who places a card.</param>
        /// <param name="cardNameToPlace">Card to be placed.</param>
        public CardPlacement(PlayerAccount player, CardName cardNameToPlace)
        {
            this.player = player;
            this.cardNameToPlace = cardNameToPlace;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => null; // placedCardName should be unknown until card open.

        public CardName? SecondTargetCardName => null;

        public CardName CardNameToPlace => cardNameToPlace;

        void IGameAction.Resolve(IMutableGame g)
        {
            var card = g.HandOf(player).RemoveCard(cardNameToPlace);
            var battle = g.CurrentBattle;
            battle.SetCard(card);
            if (player == battle.AttackingPlayer)
            {
                g.AddCommandAuthorizer(new CommandAuthorizerImplement<CardPlacement>(g.OpponentOf(player)));
            } else if (g.DetectionIsAvailableInRound(battle.Round))
            {
                g.AddCommandAuthorizer(new CommandAuthorizerImplement<Detection>(battle.DefendingPlayer));
            } else
            {
                var ap = battle.AttackingPlayer;
                g.PushToActionStack(new CardOpen(ap, battle.CardOf(ap).Name));
            }
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            var battle = g.CurrentBattle;
            // Assertion. Expected to be never thrown.
            Debug.Assert(
                !battle.HasCardOf(player),
                "Player has already set card"
                );
            var opponent = g.OpponentOf(player);
            if (player == battle.AttackingPlayer)
            {
                Debug.Assert(
                    !battle.HasCardOf(opponent),
                    "Defending player placed a card earlier than attackingPlayer"
                );
            }
            else
            {
                Debug.Assert(
                    battle.HasCardOf(opponent),
                    "Attacking player has not placed a card yet."
                );
            }
            // Assertion end.

            if (!g.HandOf(player).HasCard(cardNameToPlace))
            {
                throw new UnavailableActionException($"Invalid Command: player does not have the card to set ({cardNameToPlace})");
            }
        }

        /// <summary>
        /// Choose a card to place automatically.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller to place a card.</param>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when the cojmmandingPlayer has no card.</exception>
        public static CardPlacement CreateRandomCommand(IGameInformation g, PlayerAccount commandingPlayer)
        {
            var target = g.HandOf(commandingPlayer).SelectRandomCard();
            return new CardPlacement(commandingPlayer, target.Name);
        }
    }
}
