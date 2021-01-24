using OverroidModel.GameAction;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Innocence (1).
    /// </summary>
    public class Miracle : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.PRE_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            var oppnentCard = g.CurrentBattle.CardOf(g.OpponentOf(player));
            return oppnentCard.Name == CardName.Overroid;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new MiracleEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
