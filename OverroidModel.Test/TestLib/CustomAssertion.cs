using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.GameAction;
using Xunit;

namespace OverroidModel.Test.TestLib
{
    static class CustomAssertion
    {
        public static void CardIsJustGuessable(InGameCard c)
        {
            Assert.True(c.IsGuessable(), "The card is expected to be guessable but not so");
            Assert.False(c.IsHacked(), "The card is expected not to be hacked but so");
            Assert.False(c.IsOpened(), "The card is expected not to be opened but so");
        }

        public static void CardIsHacked(InGameCard c)
        {
            Assert.True(c.IsHacked(), "The card is expected to be hacked but not so");
            Assert.True(c.IsGuessable(), "The card is expected to be guessable but not so");
        }
        public static void CardIsOpened(InGameCard c)
        {
            Assert.True(c.IsOpened(), "The card is expected to be opened but not so");
            Assert.False(c.IsHacked(), "The card is expected not to be hacked but so");
        }

        public static void SameEffect(ICardEffect expected, ICardEffect actual)
        {
            Assert.IsType(
                expected.GetType(),
                actual
            ); 
        }
        
        public static void NotSameEffect(ICardEffect expected, ICardEffect actual)
        {
            Assert.IsNotType(
                expected.GetType(),
                actual
            ); 
        }
        
        public static void ActionIsInHistory<T>(IEnumerable<IGameAction>actions) where T : IGameAction
        {
            Assert.True(
                actions.Any(a => a is T),
                String.Format("Expected action type <{0}>is not in the history", nameof(T))
            ); 
        }

        public static void ActionIsNotInHistory<T>(IEnumerable<IGameAction>actions) where T : IGameAction
        {
            Assert.False(
                actions.Any(a => a is T),
                String.Format("Unexpected action type <{0}> is found in the history", nameof(T))
            ); 
        }

        public static void WinsInLastRound(CardName cardName, IGameInformation game)
        {
            var battle = game.Battles.Last( b => b.HasFinished() );
            if (battle.CardOf(battle.AttackingPlayer).Name != cardName && battle.CardOf(battle.DefendingPlayer).Name != cardName)
            {
                throw new Exception("CardName for assertion is not a card of last battle round");
            }
            Assert.Equal(cardName, battle.CardOf(battle.Winner!).Name);
        }

        public static void LosesInLastRound(CardName cardName, IGameInformation game)
        {
            var battle = game.Battles.Last( b => b.HasFinished() );
            if (battle.CardOf(battle.AttackingPlayer).Name != cardName && battle.CardOf(battle.DefendingPlayer).Name != cardName)
            {
                throw new Exception("CardName for assertion is not a card of last battle round");
            }
            Assert.Equal(cardName, battle.CardOf(game.OpponentOf(battle.Winner!)).Name);
        }

        public static void WaitingForCommand<T>(PlayerAccount player, IGameInformation game) where T : IGameAction
        {
            var expected = game.ExpectedCommandInfo;
            Assert.NotNull(expected);
            Assert.Equal(typeof(T), expected?.type);
            Assert.Equal(player, expected?.player);
        }

        public static void NotWaitingForCommand<T>(IGameInformation game) where T : IGameAction
        {
            var expected = game.ExpectedCommandInfo;
            if (expected == null)
            {
                return;
            }
            Assert.NotEqual(typeof(T), expected?.type);
        }
    }
}
