﻿using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;

namespace OverroidModel.GameAction.Commands
{
    public class Detection : IGameCommand
    {
        readonly PlayerAccount controller;
        readonly CardName targetCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="targetCardName">Card name to detect.</param>
        public Detection(PlayerAccount controller, CardName targetCardName)
        {
            this.controller = controller;
            this.targetCardName = targetCardName;
        }

        public PlayerAccount CommandingPlayer => controller;

        public Dictionary<string, string> ParametersToSave => throw new NotImplementedException();

        public PlayerAccount? Controller => controller;

        public CardName? TargetCardName => targetCardName;

        public CardName? SecondTargetCardName => null;

        public CardName DetectedCardName => targetCardName;

        void IGameAction.Resolve(IMutableGame g)
        {
            g.CurrentBattle.DetectCard(targetCardName);
            var b = g.CurrentBattle; 
            g.PushToActionStack(new CardOpen(b.AttackingPlayer, b.CardOf(b.AttackingPlayer).Name));
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            // no validation is required
        }

        /// <summary>
        /// Choose detected card name at random.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller of Hacker card.</param>
        public static Detection CreateRandomCommand(IGameInformation g, PlayerAccount commandingPlayer)
        {
            List<ICardInfo> cards = g.AllInGameCards.Cast<ICardInfo>().ToList();
            cards.Add(g.HiddenCard);
            var rand = new Random();
            var randomIndex = rand.Next(cards.Count - 1);
            var target = cards[randomIndex];
            return new Detection(commandingPlayer, target.Name);
        }
    }
}