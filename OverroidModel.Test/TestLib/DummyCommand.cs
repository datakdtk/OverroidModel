using OverroidModel.Card;
using OverroidModel.GameAction;
using OverroidModel.GameAction.Commands;

namespace OverroidModel.Test.TestLib
{
    class DummyCommand : IGameCommand
    {
        readonly PlayerAccount player;

        public DummyCommand(PlayerAccount player)
        {
            this.player = player;
        }

        public PlayerAccount CommandingPlayer => player;

        public PlayerAccount? Controller => player;

        public CardName? TargetCardName => null;

        public bool HasVisualEffect() => false;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(IMutableGame g)
        {
            // Do nothing
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            // Do nothing
        }
    }
}
