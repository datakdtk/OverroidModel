using OverroidModel.Game;
using OverroidModel.Game.Actions;

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

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGameInformation g)
        {
            return new CharmEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
