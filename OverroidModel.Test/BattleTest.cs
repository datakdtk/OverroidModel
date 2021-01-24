using OverroidModel.Card;
using Xunit;

namespace OverroidModel.Test
{
    public class BattleTest
    {
        const string attackingPlayerId = "hoge";
        const string defendingPlayerId = "fuga";

        [Fact]
        public void Test_InitialState_Players()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            Assert.Equal(ap, b.AttackingPlayer);
            Assert.Equal(dp, b.DefendingPlayer);
        }

        [Fact]
        public void Test_InitialState_Cards()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            Assert.False(b.HasCardOf(ap));
            Assert.False(b.HasCardOf(dp));
            Assert.Null(b.DetectedCardName);
            Assert.False(b.HasBothPlayersCards());
        }

        [Fact]
        public void Test_InitialState_BattleResult()
        {
            var b = GetBattle();
            Assert.Null(b.Winner);
            Assert.False(b.HasFinished());
            Assert.False(b.IsDrawBattle());
        }



        [Fact]
        public void Test_SetCard_AttackingPlayer()
        {
            var b = GetBattle();
            var p = new PlayerAccount(attackingPlayerId);
            Assert.False(b.HasCardOf(p));

            var c = CardDictionary.GetInGameCard(CardName.Innocence);
            var detectedCardName = CardName.Hacker; // should be ignored
            b.SetCard(p, c, detectedCardName);

            Assert.True(b.HasCardOf(p));
            Assert.Equal(c.Name, b.CardOf(c.Name).Name);
            Assert.Equal(c.Name, b.CardOf(p).Name);
            Assert.Equal(p, b.PlayerOf(c.Name));
            Assert.Null(b.DetectedCardName);
            Assert.False(b.HasBothPlayersCards());
        }

        [Fact]
        public void Test_SetCard_DefendingPlayer()
        {
            var b = GetBattle();
            var p = new PlayerAccount(defendingPlayerId);
            Assert.False(b.HasCardOf(p));

            var c = CardDictionary.GetInGameCard(CardName.Innocence);
            var detectedCardName = CardName.Hacker;
            b.SetCard(p, c, detectedCardName);

            Assert.True(b.HasCardOf(p));
            Assert.Equal(c.Name, b.CardOf(c.Name).Name);
            Assert.Equal(c.Name, b.CardOf(p).Name);
            Assert.Equal(p, b.PlayerOf(c.Name));
            Assert.Equal(b.DetectedCardName, detectedCardName);
            Assert.False(b.HasBothPlayersCards());
        }

        [Fact]
        public void Test_ReplaceCard()
        {
            var b = GetBattle();
            var p = new PlayerAccount(defendingPlayerId);

            var c1 = CardDictionary.GetInGameCard(CardName.Innocence);
            var c2 = CardDictionary.GetInGameCard(CardName.Hacker);
            var detectedCardName = CardName.Creator;
            b.SetCard(p, c1, detectedCardName);
            Assert.Equal(c1.Name, b.CardOf(p).Name);

            b.ReplaceCard(p, c2);
            Assert.Equal(c2.Name, b.CardOf(p).Name);
            Assert.Equal(b.DetectedCardName, detectedCardName);
        }

        [Fact]
        public void Test_JudgeWinnerByValues_AttackingPlayerWins()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Idol);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, null);
            Assert.True(b.HasBothPlayersCards());
            Assert.Null(b.Winner);
            Assert.False(b.HasFinished());
            Assert.False(b.IsDrawBattle());

            b.JudgeWinnerByValues();
            Assert.Equal(ap, b.Winner);
            Assert.True(b.HasFinished());
            Assert.False(b.IsDrawBattle());
        }

        [Fact]
        public void Test_JudgeWinnerByValues_DefendingPlayerWins()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Doctor);
            var dc = CardDictionary.GetInGameCard(CardName.Idol);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, null);
            Assert.True(b.HasBothPlayersCards());
            Assert.Null(b.Winner);
            Assert.False(b.HasFinished());
            Assert.False(b.IsDrawBattle());

            b.JudgeWinnerByValues();
            Assert.Equal(dp, b.Winner);
            Assert.True(b.HasFinished());
            Assert.False(b.IsDrawBattle());
        }

        [Fact]
        public void Test_JudgeWinnerByValues_DrawBattle()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Doctor);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, null);
            Assert.True(b.HasBothPlayersCards());
            Assert.Null(b.Winner);
            Assert.False(b.HasFinished());
            Assert.False(b.IsDrawBattle());

            b.JudgeWinnerByValues();
            Assert.Null(b.Winner);
            Assert.True(b.HasFinished());
            Assert.True(b.IsDrawBattle());
        }

        [Fact]
        public void Test_SetToReverseCardValues()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Spy);
            var dc = CardDictionary.GetInGameCard(CardName.Trickster);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, null);
            b.SetToReverseCardValues();

            b.JudgeWinnerByValues();
            Assert.Equal(dp, b.Winner);
        }

        [Fact]
        public void Test_SetSpecialWinner()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Idol);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, null);
            b.SetSpecialWinner(dc.Name);
            Assert.Equal(dp, b.Winner);
            Assert.True(b.HasFinished());

            b.JudgeWinnerByValues();
            Assert.Equal(dp, b.Winner);
        }

        [Fact]
        public void Test_SetToReverseCardValuesAndSetSpecialWinner()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Spy);
            var dc = CardDictionary.GetInGameCard(CardName.Trickster);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, null);
            b.SetToReverseCardValues();
            b.SetSpecialWinner(ac.Name);
            Assert.Equal(ap, b.Winner);
            Assert.True(b.HasFinished());

            b.JudgeWinnerByValues();
            Assert.Equal(ap, b.Winner);
        }

        [Fact]
        public void Test_WiningStarOf_NoDetection()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Creator);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, null);
            b.JudgeWinnerByValues();
            Assert.Equal(dp, b.Winner);

            Assert.Equal(0, b.WinningStarOf(ap));
            Assert.Equal(1, b.WinningStarOf(dp));
        }

        [Fact]
        public void Test_WiningStarOf_DetectionFailed()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Creator);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, dc.Name);
            b.JudgeWinnerByValues();
            Assert.Equal(dp, b.Winner);

            Assert.Equal(0, b.WinningStarOf(ap));
            Assert.Equal(1, b.WinningStarOf(dp));
        }

        [Fact]
        public void Test_WiningStarOf_DetectionSucceeded()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Creator);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, ac.Name);
            b.JudgeWinnerByValues();
            Assert.Equal(dp, b.Winner);

            Assert.Equal(0, b.WinningStarOf(ap));
            Assert.Equal(2, b.WinningStarOf(dp));
        }

        [Fact]
        public void Test_WiningStarOf_AttackingPlayerWins()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Idol);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor);

            b.SetCard(ap, ac, null);
            b.SetCard(dp, dc, ac.Name);
            b.JudgeWinnerByValues();
            Assert.Equal(ap, b.Winner);

            Assert.Equal(1, b.WinningStarOf(ap));
            Assert.Equal(0, b.WinningStarOf(dp));

        }

        static Battle GetBattle()
        {
            return new Battle(
                1,
                new PlayerAccount(attackingPlayerId),
                new PlayerAccount(defendingPlayerId)
                );
        }

    }
}
