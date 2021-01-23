using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Game.Actions;
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
    }
}
