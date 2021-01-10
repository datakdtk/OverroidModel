using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    public class Singularity : ICardEffect
    {
        EffectTiming ICardEffect.Timing => EffectTiming.POST_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g)
        {
            var battle = g.CurrentBattle;
            var player = battle.PlayerOf(sourceCardName);
            return battle.Winner == player;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new SingularityEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
