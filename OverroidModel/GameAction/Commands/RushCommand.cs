using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Exceptions;

namespace OverroidModel.GameAction.Commands
{
    /// <summary>
    /// Effect resolving action of Solder (11).
    /// </summary>
    public class RushCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName targetCardName;

        /// <param name="player">Card controller of the source card of the effect.</param>
        /// <param name="targetCardName"></param>
        public RushCommand(PlayerAccount player, CardName targetCardName)
        {
            this.player = player;
            this.targetCardName = targetCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => targetCardName;

        public CardName? SecondTargetCardName => null;

        public CardName RushTargetCardName => targetCardName;

        public Dictionary<string, string> ParametersToSave => new Dictionary<string, string>()
        {
            ["targetCardName"] = TargetCardName?.ToString() ?? "",
        };

        void IGameAction.Resolve(IMutableGame g)
        {
            g.HandOf(player).AddCard(g.CurrentBattle.CardOf(player));
            var targetCard = g.HandOf(player).RemoveCard(targetCardName);
            g.CurrentBattle.SetCard(targetCard);
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            if (!g.HandOf(player).HasCard(targetCardName))
            {
                throw new UnavailableActionException("Player does not have Rush target card");
            }
        }

        /// <summary>
        /// Choose target of Rush effect at random.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller of Soldier card.</param>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when the cojmmandingPlayer has no card.</exception>
        public static RushCommand CreateRandomCommand(IGameInformation g, PlayerAccount commandingPlayer)
        {
            var target = g.HandOf(commandingPlayer).SelectRandomCard();
            return new RushCommand(commandingPlayer, target.Name);
        }
    }
}
