using OverroidModel.Card;

namespace OverroidModel.GameAction.Commands
{
    public static class CommandGenerate
    {

        /// <summary>
        /// Choose target of Hack effect at random.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller of Hacker card.</param>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when the opponent has no unrevealed card.</exception>
        public static HackCommand CreateHackCommand(IGameInformation g, PlayerAccount commandingPlayer)
        {
            var opponentHand = g.HandOf(g.OpponentOf(commandingPlayer));
            var target = opponentHand.SelectRandomUnrevealCard();
            return new HackCommand(commandingPlayer, target.Name);
        }

        /// <summary>
        /// Choose target opponent card of Espionage at random.
        /// Used when player choose not get opponent's revealed card by the effect.
        /// </summary>
        /// <param name="g">Current game object.</param>
        /// <param name="commandingPlayer">Controller of Spy card.</param>
        /// <param name="targetMyCardName">Name of card in controller's hand that will be given to the opponent.</param>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when the opponent has no unrevealed card.</exception>
        public static EspionageCommand CreateRamdomEspionageCommand(IGameInformation g, PlayerAccount commandingPlayer, CardName targetMyCardName)
        {
            var opponentHand = g.HandOf(g.OpponentOf(commandingPlayer));
            var randomTarget = opponentHand.SelectRandomUnrevealCard();
            return new EspionageCommand(commandingPlayer, targetMyCardName, randomTarget.Name);
        }
    }
}
