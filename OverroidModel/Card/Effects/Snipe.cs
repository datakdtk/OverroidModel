using OverroidModel.GameAction;
using OverroidModel.GameAction.Effects;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Death (13).
    /// </summary>
    public class Snipe : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.PRE_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            var battle = g.CurrentBattle;
            return sourceCardName == battle.CardOf(battle.AttackingPlayer).Name;
        }

        ICardEffectAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new SnipeEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
