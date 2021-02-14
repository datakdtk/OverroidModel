using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Exceptions;

namespace OverroidModel.GameAction.Commands
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

        public CardName? SecondTargetCardName => null;

        public Dictionary<string, string> ParametersToSave => new Dictionary<string, string>()
        {
            ["targetCardName"] = TargetCardName?.ToString() ?? "",
        };

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

        /// <summary>
        /// Choose target of Morph effect at random.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller of Beast card.</param>
        /// <exception cref="UnavailableActionException">Thrown this is first round.</exception>
        public static MorphCommand CreateRandomCommand(IGameInformation g, PlayerAccount commandingPlayer)
        {
            var preveousRound = g.CurrentBattle.Round - 1;
            if (preveousRound <= 0)
            {
                throw new UnavailableActionException("There are no battle cards to be Morph target.");
            }
            var rand = new Random();
            var randomRound = rand.Next(1, preveousRound + 1);
            var target = g.Battles[randomRound - 1].CardOf(g.OpponentOf(commandingPlayer)); ;
            if (target == null)
            {
                throw new GameLogicException("Failed to choose Morph target at random. Not found");
            }
            return new MorphCommand(commandingPlayer, target.Name);
        }
    }
}
