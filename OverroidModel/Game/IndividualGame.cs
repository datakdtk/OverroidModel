using OverroidModel.Config;
using OverroidModel.Exceptions;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel.Game
{
    /// <summary>
    /// Implementation of one-to-one game.
    /// </summary>
    public class IndividualGame : IMutableGame
    {
        readonly PlayerAccount humanPlayer;
        readonly PlayerAccount overroidPlayer;
        readonly Dictionary<PlayerAccount, PlayerHand> playerHands;
        readonly IGameConfig config;
        readonly List<Battle> battles;
        readonly Stack<IGameAction> actionStack;
        readonly List<IGameAction> actionHistory;
        readonly PlayerAccount?[] effectDisabledPlayers;
        ICommandAuthorizer? commandAuthorizer;
        PlayerAccount? specialWinner;

        /// <param name="humanPlayer"> Player who plays Human force,</param>
        /// <param name="overroidPlayer">Player who plays Overroid force.</param>
        /// <param name="humanPlayerHand">Hand cards of Human player.</param>
        /// <param name="overroidPlayerHand">Hand card of Overroid player.</param>
        /// <param name="config">Customized rule of the game</param>
        internal IndividualGame(
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            PlayerHand humanPlayerHand,
            PlayerHand overroidPlayerHand,
            IGameConfig config
        )
        {
            if (humanPlayer == overroidPlayer)
            {
                throw new ArgumentException("Two game players are same.");
            }
            this.humanPlayer = humanPlayer;
            this.overroidPlayer = overroidPlayer;
            this.config = config;

            playerHands = new Dictionary<PlayerAccount, PlayerHand>()
            {
                [humanPlayer] = humanPlayerHand,
                [overroidPlayer] = overroidPlayerHand,
            };
            
            this.battles = battles ?? new List<Battle>();
            actionStack = new Stack<IGameAction>();
            actionHistory = new List<IGameAction>();
            effectDisabledPlayers = new PlayerAccount?[IGameInformation.maxRound];
        }

        public PlayerAccount HumanPlayer => humanPlayer;
        public PlayerAccount OverroidPlayer => overroidPlayer;
        public IReadOnlyList<IGameAction> ActionHistory => actionHistory;
        public IReadOnlyList<Battle> Battles => battles;
        public Battle CurrentBattle => battles.Count > 0 ? battles.Last() : throw new GameLogicException("Game has no battle round yet.");
        public PlayerAccount? Winner
        {
            get 
            {
                if (!HasFinished())
                {
                    return null;
                }
                if (specialWinner != null)
                {
                    return specialWinner;
                }
                var humanStars = WinningStarOf(humanPlayer);
                var overroidStars = WinningStarOf(overroidPlayer);
                if (humanStars == overroidStars)
                {
                    return null;
                }
                return humanStars > overroidStars ? humanPlayer : overroidPlayer;
            }
        }

        public bool DetectionAvailable => config.DetectionAvailable;
        public (Type type, PlayerAccount player)? ExpectedCommandInfo => commandAuthorizer?.RequiredCommandInfo;


        public bool HasFinished()
        {
            if (specialWinner != null)
            {
                return true;
            }
            if (Battles.Count > IGameInformation.maxRound)
            {
                throw new GameLogicException("Number of battles in the game exceeds max round");
            }
            return Battles.Count == IGameInformation.maxRound && CurrentBattle.HasFinished();
                
        }

        public PlayerAccount OpponentOf(PlayerAccount p)
        {
            if (p == humanPlayer)
            {
                return overroidPlayer;
            }
            if (p == overroidPlayer)
            {
                return humanPlayer;
            }
            throw new NonGamePlayerException(p);
        }

        public PlayerHand HandOf(PlayerAccount p)
        {
            if (!playerHands.ContainsKey(p))
            {
                throw new NonGamePlayerException(p);
            }
            return playerHands[p];
        }

        public ushort WinningStarOf(PlayerAccount p) => (ushort)battles.Aggregate(0, (c, b) => (c + b.WinningStarOf(p)));

        public void ReceiveCommand(IGameCommand command)
        {
            if (commandAuthorizer == null)
            {
                throw new UnavailableActionException("The game does not accept any command now.");
            }

            commandAuthorizer.Authorize(command);
            command.Validate(this);
            PushToActionStack(command);
            commandAuthorizer = null;
            ResolveStacks();
        }

        void IMutableGame.AddNewRound()
        {
            if (HasFinished())
            {
                throw new UnavailableActionException("New round cannot start. This is last round");
            }
            var newRound = Battles.Count > 0 ? (ushort)(CurrentBattle.Round + 1) : (ushort)1;
            var attackingPlayer = newRound % 2 == 1 ? overroidPlayer : humanPlayer;
            var defendingPlayer = newRound % 2 == 1 ? humanPlayer : overroidPlayer;
            battles.Add(new Battle(newRound, attackingPlayer, defendingPlayer));
        }

        void IMutableGame.AddCommandAuthorizer(ICommandAuthorizer a) => commandAuthorizer = a;

        void IMutableGame.DisableRoundEffects(PlayerAccount targetPlayer, ushort round)
        {
            if (round == 0 || round > IGameInformation.maxRound)
            {
                throw new ArgumentOutOfRangeException("Failed to disable round effect because out of round value");
            }
            effectDisabledPlayers[round] = targetPlayer;
        }

        void IMutableGame.PushToActionStack(IGameAction a) => PushToActionStack(a);

        void IMutableGame.SetSpecialWinner(PlayerAccount p) => specialWinner = p;

        internal void ResolveStacks()
        {
            while(commandAuthorizer == null && actionStack.Count > 0)
            {
                if (HasFinished())
                {
                    actionStack.Clear();
                    break;
                }
                var a = actionStack.Pop();

                if (a.IsCardEffect() && effectDisabledPlayers[CurrentBattle.Round] == a.Controller)
                {
                    continue;
                }
                a.Resolve(this);
                actionHistory.Add(a);
            }
        }

        internal void PushToActionStack(IGameAction a)
        {
            actionStack.Push(a);
        }
    }
}
