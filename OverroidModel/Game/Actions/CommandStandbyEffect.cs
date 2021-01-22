using OverroidModel.Card;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game.Actions
{
    /// <summary>
    /// Action to get readied to accept external commands.
    /// </summary>
    /// <typeparam name="T">Expected command class.</typeparam>
    public class CommandStandbyEffect<T> : IGameAction where T : IGameCommand
    {
        readonly CardName sourceCardName;

        public CommandStandbyEffect(CardName sourceCardName)
        {
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount? Controller => null;

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
