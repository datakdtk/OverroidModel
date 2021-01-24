using OverroidModel.GameAction.Effects;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Overroid (12).
    /// </summary>
    public class Singularity : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            var battle = g.CurrentBattle;
            var player = battle.PlayerOf(sourceCardName);
            return battle.Winner == player;
        }

        ICardEffectAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new SingularityEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
