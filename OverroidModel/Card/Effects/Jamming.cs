using OverroidModel.GameAction;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Doctor (4).
    /// </summary>
    public class Jamming : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.FIRST;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g) => true;

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new JammingEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
