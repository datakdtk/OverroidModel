using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Spy (7).
    /// </summary>
    public class Espionage : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            return g.HandOf(g.HumanPlayer).Count >= 1 && g.HandOf(g.OverroidPlayer).Count >= 1;
        }

        ICardEffectAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            return new CommandStandbyEffect<EspionageCommand>(sourceCardName, player);
        }
    }
}
