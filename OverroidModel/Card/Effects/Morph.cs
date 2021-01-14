using OverroidModel.Game;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;

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

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new CommandStandbyEffect<MorphCommand>(sourceCardName);
        }
    }
}
