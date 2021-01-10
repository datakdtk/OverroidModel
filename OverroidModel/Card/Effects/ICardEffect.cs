using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    internal enum EffectTiming
    {
        FIRST = 0,
        SECOND = 1,
        PRE_BATTLE = 2,
        POST_BATTLE = 3,
    }

    public interface ICardEffect
    {
        internal EffectTiming Timing { get; }

        internal IGameAction GetAction(CardName sourceCardName, IGame g);

        internal bool ConditionIsSatisfied(CardName sourceCardName, IGame g);
    }
}
