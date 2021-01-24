using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;
using OverroidModel.GameAction;
using Xunit;

namespace OverroidModel.Test.TestLib
{
    static class CustomAssertion
    {
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
