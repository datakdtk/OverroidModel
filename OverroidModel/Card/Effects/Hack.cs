using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Hacker (2).
    /// </summary>
    public class Hack : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            var opponentHand = g.HandOf(g.OpponentOf(player));
            return opponentHand.UnrevealedCardCount >= 1;
        }

        ICardEffectAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            return new CommandStandbyEffect<HackCommand>(sourceCardName, player);
        }
    }
}
