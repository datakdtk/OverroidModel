﻿using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card.Master;

namespace OverroidModel.Test.TestLib
{
    class DummyShuffler : ICardShuffler
    {
        public List<ICardMaster> Shuffle(IEnumerable<ICardMaster> cards, string seed)
        {
            return cards.ToList();
        }
    }
}
