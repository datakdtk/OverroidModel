using OverroidModel.Game;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Soldier (11).
    /// </summary>
    public class Rush : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGameInformation g)
        {
            var battle = g.CurrentBattle;
            var player = battle.PlayerOf(sourceCardName);
            return battle.Winner == player && g.HandOf(player).Count >= 1;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new CommandStandbyEffect<RushCommand>(sourceCardName);
        }
    }
}
