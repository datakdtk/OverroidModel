﻿using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    class NoAction : IGameAction
    {
        public PlayerAccount? Controller => null;

        public CardName? TargetCardName => null;

        public bool HasVisualEffect() => false;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(IMutableGame g)
        {
            return; // Do nothing.
        }
    }
}