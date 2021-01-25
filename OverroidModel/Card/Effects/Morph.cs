using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Beast (9).
    /// </summary>
    public class Morph : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.SECOND;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            return g.CurrentBattle.Round >= 2;
        }

        ICardEffectAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            return new CommandStandbyEffect<MorphCommand>(sourceCardName, player);
        }
    }
}
