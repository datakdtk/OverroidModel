﻿using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Legion (10).
    /// </summary>
    public class Trample : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            var battle = g.CurrentBattle;
            var player = battle.PlayerOf(sourceCardName);
            return battle.Winner == player && !g.HasFinished();
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new TrampleEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
