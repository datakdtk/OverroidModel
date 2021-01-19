using OverroidModel.Card;
using OverroidModel.Card.Master;
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
    internal class IndividualGame : IMutableGame
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

        const ushort maxRound = 6;

        /// <param name="humanPlayer"> Player who plays Human force,</param>
        /// <param name="overroidPlayer">Player who plays Overroid force.</param>
        /// <param name="humanPlayerHand">Hand cards of Human player.</param>
        /// <param name="overroidPlayerHand">Hand card of Overroid player.</param>
        internal IndividualGame(
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            List<ICardMaster> sortedDeck,
            IGameConfig config
        )
        {
            this.humanPlayer = humanPlayer;
            this.overroidPlayer = overroidPlayer;
            this.config = config;

            var humanHand = sortedDeck.GetRange(0, 5).Select(c => new InGameCard(c)).ToList();
            var overroidHand = sortedDeck.GetRange(5, 5).Select(c => new InGameCard(c)).ToList();
            humanHand.Add(new InGameCard(new Innocence()));
            overroidHand.Add(new InGameCard(new Overroid()));
            playerHands = new Dictionary<PlayerAccount, PlayerHand>()
            {
                [humanPlayer] = new PlayerHand(humanHand),
                [overroidPlayer] = new PlayerHand(overroidHand),
            };
            
            battles = new List<Battle>();
            actionStack = new Stack<IGameAction>();
            actionHistory = new List<IGameAction>();
            effectDisabledPlayers = new PlayerAccount?[maxRound];
        }

        public PlayerAccount HumanPlayer => humanPlayer;
        public PlayerAccount OverroidPlayer => overroidPlayer;
        public List<IGameAction> ActionHistory => actionHistory;
        public IReadOnlyList<Battle> Battles => battles;
        public Battle CurrentBattle => battles.Last();
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

        public bool HasFinished()
        {
            if (specialWinner != null)
            {
                return true;
            }
            return CurrentBattle.Round == maxRound && CurrentBattle.HasFinished();
                
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
            ResolveStacks();
        }

        void IMutableGame.AddNewRound()
        {
            if (HasFinished())
            {
                throw new UnavailableActionException("New round cannot start. This is last round");
            }
            var newRound = (ushort)(CurrentBattle.Round + 1);
            var attackingPlayer = newRound % 2 == 1 ? overroidPlayer : humanPlayer;
            var defendingPlayer = newRound % 2 == 1 ? humanPlayer : overroidPlayer;
            battles.Add(new Battle(newRound, attackingPlayer, defendingPlayer));
        }

        void IMutableGame.AddCommandAuthorizer(ICommandAuthorizer a) => commandAuthorizer = a;

        void IMutableGame.DisableRoundEffects(PlayerAccount targetPlayer, ushort round)
        {
            if (round == 0 || round > maxRound)
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
                var effectsDisabled = effectDisabledPlayers[CurrentBattle.Round] == a.Controller;
                if (effectsDisabled && a.IsCardEffect())
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
