using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Hacker (2).
    /// </summary>
    public class Hack : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            var opponentHand = g.HandOf(g.OpponentOf(player));
            return opponentHand.UnrevealedCardCount >= 1;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new HackEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
