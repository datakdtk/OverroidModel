using OverroidModel.GameAction.Effects;

namespace OverroidModel.Card.Effects
{
    public class NoEffect : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g) => false;

        ICardEffectAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new NoEffectAction(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }

    }
}
