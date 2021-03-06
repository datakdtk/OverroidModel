﻿using OverroidModel.Card;
using OverroidModel.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;

namespace OverroidModel.GameAction.Commands
{
    /// <summary>
    /// Effect resolving action of Spy (7).
    /// </summary>
    public class EspionageCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName targetMyCardName;
        readonly CardName targetOpponentCardName;

        /// <param name="player">Card controller of the source card of the effect.</param>
        /// <param name="targetMyCardName">Name of card which will be chosen from the card controller's hand.</param>
        /// <param name="targetOpponentCardName">Name of card which will be chosen from the opponent's hand.</param>
        public EspionageCommand(PlayerAccount player, CardName targetMyCardName, CardName targetOpponentCardName)
        {
            this.player = player;
            this.targetMyCardName = targetMyCardName;
            this.targetOpponentCardName = targetOpponentCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => targetMyCardName;

        public CardName? SecondTargetCardName => targetOpponentCardName;

        public CardName TargetMyCardName => targetMyCardName;

        public CardName TargetOpponentCardName => targetOpponentCardName;

        void IGameAction.Resolve(IMutableGame g)
        {
            var myHand = g.HandOf(player);
            var opponentHand = g.HandOf(g.OpponentOf(player));
            Debug.Assert(myHand.Cards.Count == opponentHand.Cards.Count);
            var myCard = myHand.RemoveCard(targetMyCardName);
            var opponentCard = opponentHand.RemoveCard(targetOpponentCardName);

            myCard.ChangeOwner(opponentHand.Player);
            opponentCard.ChangeOwner(myHand.Player);

            myHand.AddCard(opponentCard);
            opponentHand.AddCard(myCard);
            Debug.Assert(myHand.Cards.Count == opponentHand.Cards.Count);
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            if (!g.HandOf(player).HasCard(targetMyCardName))
            {
                throw new UnavailableActionException("Invalid Command: player does not have the card to exchange");
            }
            var opponentHand = g.HandOf(g.OpponentOf(player));
            if (!opponentHand.HasCard(targetOpponentCardName))
            {
                throw new UnavailableActionException("Invalid Command: opponent does not have the card to exchange");
            }
        }

        /// <summary>
        /// Choose targets Espionage at random for both players hand.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller of Spy card.</param>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when either player has no card.</exception>
        public static EspionageCommand CreateRandomCommand(IGameInformation g, PlayerAccount commandingPlayer)
        {
            var myTarget = g.HandOf(commandingPlayer).SelectRandomCard();
            var opponentTarget = g.HandOf(g.OpponentOf(commandingPlayer)).SelectRandomCard();
            return new EspionageCommand(commandingPlayer, myTarget.Name, opponentTarget.Name);
        }

        /// <summary>
        /// Choose target opponent card of Espionage at random.
        /// Used when player choose not get opponent's revealed card by the effect.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller of Spy card.</param>
        /// <param name="targetMyCardName">Name of card in controller's hand that will be given to the opponent.</param>
        /// <exception cref="Exceptions.CardNotFoundException">Thrown when the commandingPlayer has given target card.</exception>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when the opponent has no unrevealed card.</exception>
        public static EspionageCommand CreateRandomCommandWithMyCardChoice(IGameInformation g, PlayerAccount commandingPlayer, CardName targetMyCardName)
        {
            if (!g.HandOf(commandingPlayer).HasCard(targetMyCardName))
            {
                throw new CardNotFoundException("commanding player does not have target card for espionage.");
            }
            var opponentHand = g.HandOf(g.OpponentOf(commandingPlayer));
            var randomTarget = opponentHand.SelectRandomUnrevealCard();
            return new EspionageCommand(commandingPlayer, targetMyCardName, randomTarget.Name);
        }

    }
}
