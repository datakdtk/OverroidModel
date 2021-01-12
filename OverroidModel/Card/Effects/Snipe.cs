using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Death (13).
    /// </summary>
    public class Snipe : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.PRE_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g)
        {
            var battle = g.CurrentBattle;
            return sourceCardName == battle.CardOf(battle.AttackingPlayer).Name;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new SnipeEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
