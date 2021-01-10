using OverroidModel.Exceptions;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel.Game
{
    internal class IndividualGame : IGame
    {
        readonly PlayerAccount humanPlayer;
        readonly PlayerAccount overroidPlayer;
        readonly Dictionary<PlayerAccount, PlayerHand> playerHands;
        readonly List<Battle> battles;
        readonly Stack<IGameAction> actionStack;
        readonly List<IGameAction> actionHistory;
        readonly PlayerAccount?[] effectDisabledPlayers;
        ICommandAuthorizer? commandAuthorizer;
        PlayerAccount? specialWinner;

        const ushort maxRound = 6;
        

        public IndividualGame(
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            PlayerHand humanPlayerHand,
            PlayerHand overroidPlayerHand
        )
        {
            this.humanPlayer = humanPlayer;
            this.overroidPlayer = overroidPlayer;
            playerHands = new Dictionary<PlayerAccount, PlayerHand>()
            {
                [humanPlayer] = humanPlayerHand,
                [overroidPlayer] = overroidPlayerHand,
            };
            battles = new List<Battle>();
            actionStack = new Stack<IGameAction>();
            actionHistory = new List<IGameAction>();
            effectDisabledPlayers = new PlayerAccount?[maxRound];

            PushToActionStack(new RoundStart());
            ResolveStacks();
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
            PushToActionStack(command);
            ResolveStacks();
        }

        void IGame.AddNewRound()
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

        void IGame.AddCommandAuthorizer(ICommandAuthorizer a) => commandAuthorizer = a;

        void IGame.DisableRoundEffects(PlayerAccount targetPlayer, ushort round)
        {
            if (round == 0 || round > maxRound)
            {
                throw new ArgumentOutOfRangeException("Failed to disable round effect because out of round value");
            }
            effectDisabledPlayers[round] = targetPlayer;
        }

        void IGame.PushToActionStack(IGameAction a) => PushToActionStack(a);

        void IGame.SetSpecialWinner(PlayerAccount p) => specialWinner = p;

        private void PushToActionStack(IGameAction a)
        {
            actionStack.Push(a);
        }

        private void ResolveStacks()
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
    }
}
