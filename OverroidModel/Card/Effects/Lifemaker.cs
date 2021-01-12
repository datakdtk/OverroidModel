using OverroidModel.Game;
using OverroidModel.Game.Actions;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Effect of Creator (3).
    /// </summary>
    public class Lifemaker : ICardEffect
    {

        EffectTiming ICardEffect.Timing => EffectTiming.PRE_BATTLE;

        bool ICardEffect.ConditionIsSatisfied(CardName sourceCardName, IGame g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            var oppnentCard = g.CurrentBattle.CardOf(g.OpponentOf(player));
            return oppnentCard.Name == CardName.Beast || oppnentCard.Name == CardName.Overroid;
        }

        IGameAction ICardEffect.GetAction(CardName sourceCardName, IGame g)
        {
            return new LifemakerEffect(g.CurrentBattle.PlayerOf(sourceCardName), sourceCardName);
        }
    }
}
