
using System.Collections.Generic;
using OverroidModel.Card.Master;
using OverroidModel.Exceptions;

namespace OverroidModel.Card
{
    static class CardDictionary
    {
        public static Dictionary<CardName, ICardMaster> dic;

        static CardDictionary()
        {
            dic = new Dictionary<CardName, ICardMaster>()
            {
                [CardName.Watcher] = new Watcher(),
                [CardName.Innocence] = new Innocence(),
                [CardName.Hacker] = new Hacker(),
                [CardName.Creator] = new Creator(),
                [CardName.Doctor] = new Doctor(),
                [CardName.Idol] = new Idol(),
                [CardName.Trickster] = new Trickster(),
                [CardName.Spy] = new Spy(),
                [CardName.Diva] = new Diva(),
                [CardName.Beast] = new Beast(),
                [CardName.Legion] = new Legion(),
                [CardName.Soldier] = new Soldier(),
                [CardName.Overroid] = new Overroid(),
                [CardName.Death] = new Death(),
            };
        }

        public static ICardMaster GetMaster(CardName n)
        {
            if (n == CardName.Unknown)
            {
                throw new GameLogicException("Cannot get unknown card by name");
            }
            return dic[n];
        }

        public static InGameCard GetInGameCard(CardName n)
        {
            return new InGameCard(GetMaster(n));
        }

        public static List<ICardMaster> DefaultCardList => new List<ICardMaster>()
            {
                new Hacker(),
                new Creator(),
                new Doctor(),
                new Idol(),
                new Trickster(),
                new Spy(),
                new Diva(),
                new Beast(),
                new Legion(),
                new Soldier(),
                new Death(),
            };

        public static List<ICardMaster> DefaultCardListPlusWatcher
        {
            get
            {
                var set = DefaultCardList;
                set.Add(GetMaster(CardName.Watcher));
                return set;
            }
        }
    }
}

