using System.Linq;
using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Exceptions;

namespace OverroidModel.Game.Actions.Commands
{
    /// <summary>
    /// Effect resolving action of Beast (9).
    /// </summary>
    public class MorphCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName targetCardName;

        /// <param name="player">Card controller of the source card of the effect.</param>
        /// <param name="targetCardName">Name of target card from which an effect will be copied.</param>
        public MorphCommand(PlayerAccount player, CardName targetCardName)
        {
            this.player = player;
            this.targetCardName = targetCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => targetCardName;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(IMutableGame g)
        {
            var thisCard = g.CurrentBattle.CardOf(player);
            var copiedEffect = CardDictionary.GetMaster(targetCardName).Effect;
            thisCard.OverrideEffect(copiedEffect);

            // Resolve the effect immediately if a pre-battle effect is copied.
            if (copiedEffect.Timing <= EffectTiming.PRE_BATTLE && copiedEffect.ConditionIsSatisfied(thisCard.Name, g))
            {
                g.PushToActionStack(copiedEffect.GetAction(thisCard.Name, g));
            }
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            var opponent = g.OpponentOf(player);
            var battle = g.Battles.FirstOrDefault(b => b.CardOf(opponent).Name == targetCardName);
            if (battle == null)
            {
                throw new UnavailableActionException("Morph target is not opponent's battle card.");
            }
            if (battle.Round >= g.CurrentBattle.Round)
            {
                throw new UnavailableActionException("Morph target round must be previous rounds");
            }
        }
    }
}
