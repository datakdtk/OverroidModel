using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    public class Reversal : ICardEffect
    {
        EffectTiming ICardEffect.Timing => EffectTiming.PRE_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g) => true;

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new ReversalEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
