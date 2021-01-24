using OverroidModel.GameAction;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Diva (8).
    /// </summary>
    public class Inspiration : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.PRE_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            var oppnentCard = g.CurrentBattle.CardOf(g.OpponentOf(player));
            return oppnentCard.Name == CardName.Legion;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new InspirationEfect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
