using OverroidModel.GameAction.Effects;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Watcher (0). Effect name is not official.
    /// </summary>
    public class Predict : ICardEffect
    {
        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            return true; // Always triggers.
        }

        ICardEffectAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new PredictEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
