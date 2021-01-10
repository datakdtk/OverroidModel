using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    public class Jamming : ICardEffect
    {
        EffectTiming ICardEffect.Timing => EffectTiming.FIRST;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g) => true;

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new JammingEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
