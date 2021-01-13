using OverroidModel.Game;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Beast (9).
    /// </summary>
    public struct Morph : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.SECOND;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g)
        {
            return g.CurrentBattle.Round >= 2;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new CommandStandbyEffect<MorphCommand>(sourceCardName);
        }
    }
}
