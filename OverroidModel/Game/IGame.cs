using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
using System.Collections.Generic;

namespace OverroidModel.Game
{
    public interface IGame
    {
        public PlayerAccount HumanPlayer { get; }
        public PlayerAccount OverroidPlayer { get; }
        public List<IGameAction> ActionHistory { get; }
        public IReadOnlyList<Battle> Battles { get; }
        public Battle CurrentBattle { get; }
        public PlayerAccount? Winner{ get; }

        public bool HasFinished();

        public bool IsDrawGame() => HasFinished() && Winner == null;

        public PlayerAccount OpponentOf(PlayerAccount p);

        public PlayerHand HandOf(PlayerAccount p);

        public ushort WinningStarOf(PlayerAccount p);

        public void ReceiveCommand(IGameCommand command);

        internal void AddNewRound();

        internal void AddCommandAuthorizer(ICommandAuthorizer a);

        internal void DisableRoundEffects(PlayerAccount targetPlayer, ushort round);

        internal void PushToActionStack(IGameAction a);

        internal void SetSpecialWinner(PlayerAccount p);
    }
}
