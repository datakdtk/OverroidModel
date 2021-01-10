using OverroidModel.Card;
using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game.Actions
{
    public class CommandStandbyEffect<T> : IGameAction where T : IGameCommand
    {
        readonly CardName sourceCardName;

        public CommandStandbyEffect(CardName sourceCardName)
        {
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount? Controller => null;

        public bool HasVisualEffect() => false;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            g.AddCommandAuthorizer(new CommandAuthorizer<T>(player));
        }
    }
}
