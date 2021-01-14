using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Trickster (6).
    /// </summary>
    public class Reversal : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.PRE_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g) => true;

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new ReversalEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
