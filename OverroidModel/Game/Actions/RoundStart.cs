﻿using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game.Actions
{
    public class RoundStart : IGameAction
    {
        public PlayerAccount? Controller => null;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(in IGame g)
        {
            g.AddNewRound();
            g.AddCommandAuthorizer(new CommandAuthorizer<CardPlacement>(g.CurrentBattle.AttackingPlayer));
        }
    }
}
