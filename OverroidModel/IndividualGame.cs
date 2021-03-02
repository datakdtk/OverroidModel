using OverroidModel.Card;
using OverroidModel.Card.Master;
using OverroidModel.Exceptions;
using OverroidModel.GameAction;
using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OverroidModel
{

    /// <summary>
    /// Implementation of one-to-one game.
    /// </summary>
    public class IndividualGame : IMutableGame
    {
        public static readonly ushort MAX_ROUND = 6;

        readonly PlayerAccount humanPlayer;
        readonly PlayerAccount overroidPlayer;
        readonly Dictionary<PlayerAccount, PlayerHand> playerHands;
        readonly OutsideCard hiddendCard;
        readonly OutsideCard? triggerCard;
        readonly IGameConfig config;
        readonly List<Battle> battles;
        readonly Stack<IGameAction> actionStack;
        readonly List<IGameAction> actionHistory;
        readonly PlayerAccount?[] effectDisabledPlayers;
        ICommandAuthorizer? commandAuthorizer;
        PlayerAccount? specialWinner;
        readonly List<InGameCard> inGameCards;

        /// <param name="humanPlayer"> Player who plays Human force,</param>
        /// <param name="overroidPlayer">Player who plays Overroid force.</param>
        /// <param name="humanPlayerHand">Hand cards of Human player.</param>
        /// <param name="overroidPlayerHand">Hand card of Overroid player.</param>
        /// <param name="hiddenCardMaster">Card that was not distributed to hands</param>
        /// <param name="triggerCardMaster">Another card that was not distributed to hand if exists.</param>
        /// <param name="config">Customized rule of the game</param>
        internal IndividualGame(
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            PlayerHand humanPlayerHand,
            PlayerHand overroidPlayerHand,
            ICardMaster hiddenCardMaster,
            ICardMaster? triggerCardMaster,
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
            
            hiddendCard = new OutsideCard(hiddenCardMaster);
            if (triggerCardMaster != null)
            {
                triggerCard = new OutsideCard(triggerCardMaster);
                triggerCard.Open();
            }

            playerHands = new Dictionary<PlayerAccount, PlayerHand>()
            {
                [humanPlayer] = humanPlayerHand,
                [overroidPlayer] = overroidPlayerHand,
            };

            battles = new List<Battle>();
            actionStack = new Stack<IGameAction>();
            actionHistory = new List<IGameAction>();
            effectDisabledPlayers = new PlayerAccount?[MAX_ROUND];

            inGameCards = humanPlayerHand.Cards.Concat(overroidPlayerHand.Cards).ToList();
        }

        public PlayerAccount HumanPlayer => humanPlayer;
        public PlayerAccount OverroidPlayer => overroidPlayer;
        public IGameConfig Config => config;
        public OutsideCard HiddenCard => hiddendCard;
        public OutsideCard? TriggerCard => triggerCard;
        public IReadOnlyList<IGameAction> ActionHistory => actionHistory;
        public IReadOnlyList<Battle> Battles => battles;
        public Battle CurrentBattle => battles.Count > 0 ? battles.Last() : throw new GameLogicException("Game has no battle round yet.");
        public ICommandRequirement? CommandRequirement => commandAuthorizer?.CommandRequirement;
        public IReadOnlyList<InGameCard> AllInGameCards => inGameCards;

        public ushort WinningStarOf(PlayerAccount p) => (ushort)battles.Aggregate(0, (c, b) => c + b.WinningStarOf(p));

        public bool HasFinished()
        {
            if (specialWinner != null)
            {
                return true;
            }
            if (Battles.Count > MAX_ROUND)
            {
                throw new GameLogicException("Number of battles in the game exceeds max round");
            }
            return Battles.Count == MAX_ROUND && CurrentBattle.HasFinished();
        }

        public PlayerAccount? CheckWinner()
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

        public bool IsDrawGame() => HasFinished() && CheckWinner() == null;

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

        public bool DetectionIsAvailableInRound(ushort round) => config.DetectionAvailable && round <= 4;

        public bool EffectIsDisabled(ushort round, PlayerAccount? p)
        {
            // Make sure at least one battle has been created and player is not null.
            if (p == null || Battles.Count == 0)
            {
                return false;
            }

            return p == effectDisabledPlayers[round];
        }

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
            ResolveNextAction();
        }

        public IGameAction? ResolveNextAction()
        {
            if (HasFinished())
            {
                actionStack.Clear();
                return null;
            }
            
            if (commandAuthorizer != null || actionStack.Count == 0)
            {
                return null;
            }

            var action = actionStack.Pop();

            var effect = action as ICardEffectAction;
            if (effect != null && EffectIsDisabled(CurrentBattle.Round, CurrentBattle.PlayerOf(effect.SourceCardName)))
            {
                return ResolveNextAction(); // Ignore effect action and go next
            }

            action.Resolve(this);
            actionHistory.Add(action);
            return action;
        }

        public void ResolveAllActions()
        {
            while (ResolveNextAction() != null);
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

        void IMutableGame.DisableRoundEffects(ushort round, PlayerAccount targetPlayer)
        {
            if (round == 0 || round > MAX_ROUND)
            {
                throw new ArgumentOutOfRangeException("Failed to disable round effect because out of round value");
            }
            effectDisabledPlayers[round] = targetPlayer;
            Debug.Assert(EffectIsDisabled(round, targetPlayer));
        }

        void IMutableGame.PushToActionStack(IGameAction a) => PushToActionStack(a);

        void IMutableGame.SetSpecialWinner(PlayerAccount p) => specialWinner = p;

        internal void PushToActionStack(IGameAction a)
        {
            actionStack.Push(a);
        }
    }
}
