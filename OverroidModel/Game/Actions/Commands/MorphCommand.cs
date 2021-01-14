using OverroidModel.Card;
using OverroidModel.Exceptions;

namespace OverroidModel.Game.Actions.Commands
{
    /// <summary>
    /// Effect resolving action of Beast (9).
    /// </summary>
    public class MorphCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly ushort targetRound;
        readonly CardName targetCardName;

        /// <param name="player">Card controller of the source card of the effect.</param>
        /// <param name="targetRound">Round where copy target card is placed.</param>
        /// <param name="targetCardName">Name of target card from which an effect will be copied.</param>
        public MorphCommand(PlayerAccount player, ushort targetRound, CardName targetCardName)
        {
            this.player = player;
            this.targetRound = targetRound;
            this.targetCardName = targetCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => targetCardName;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(IMutableGame g)
        {
            var battle = g.Battles[targetRound];
            var targetCard = battle.CardOf(g.OpponentOf(CommandingPlayer));
            var thisCard = g.CurrentBattle.CardOf(player);
            var copiedEffect = targetCard.DefaultEffect;
            thisCard.OverrideEffect(copiedEffect);

            // Push copied effect to Stack if original effect timing has already missed
            var currentTiming = thisCard.DefaultEffect.Timing;
            if (copiedEffect.Timing <= currentTiming && copiedEffect.ConditionIsSatisfied(thisCard.Name, g))
            {
                g.PushToActionStack(copiedEffect.GetAction(thisCard.Name, g));
            }
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            if (targetRound >= g.CurrentBattle.Round)
            {
                throw new UnavailableActionException("Morph target round must be previous rounds");
            }
            var targetCard = g.Battles[targetRound].CardOf(g.OpponentOf(CommandingPlayer));
            if (targetCard.Name != targetCardName)
            {
                throw new UnavailableActionException("Morph target is not opponent card of target round");
            }
        }
    }
}
