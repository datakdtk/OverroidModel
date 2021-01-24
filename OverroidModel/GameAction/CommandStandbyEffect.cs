using OverroidModel.Card;
using OverroidModel.GameAction.Commands;

namespace OverroidModel.GameAction
{
    /// <summary>
    /// Action to get readied to accept external commands.
    /// </summary>
    /// <typeparam name="T">Expected command class.</typeparam>
    public class CommandStandbyEffect<T> : IGameAction where T : IGameCommand
    {
        readonly CardName sourceCardName;
        readonly PlayerAccount player;

        public CommandStandbyEffect(CardName sourceCardName, PlayerAccount player)
        {
            this.sourceCardName = sourceCardName;
            this.player = player;
        }

        public PlayerAccount? Controller => player;

        public CardName? TargetCardName => null;

        public bool HasVisualEffect() => false;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(IMutableGame g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            g.AddCommandAuthorizer(new CommandAuthorizer<T>(player));
        }
    }
}
