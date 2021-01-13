﻿using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Spy (7).
    /// </summary>
    public struct Espionage : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g)
        {
            return g.HandOf(g.HumanPlayer).Count >= 1 && g.HandOf(g.OverroidPlayer).Count >= 1;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new CharmEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}