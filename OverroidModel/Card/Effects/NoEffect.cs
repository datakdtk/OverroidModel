using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    public class NoEffect : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g) => false;

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g) => new NoAction();

    }
}
