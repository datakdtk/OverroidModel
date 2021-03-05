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

            var c = CardDictionary.GetInGameCard(CardName.Innocence, p);
            b.SetCard(c);

            Assert.True(b.HasCardOf(p));
            Assert.Equal(c.Name, b.CardOf(c.Name).Name);
            Assert.Equal(c.Name, b.CardOf(p).Name);
            Assert.Equal(p, b.PlayerOf(c.Name));
        }

        [Fact]
        public void Test_SetCard_DefendingPlayer()
        {
            var b = GetBattle();
            var p = new PlayerAccount(defendingPlayerId);
            Assert.False(b.HasCardOf(p));

            var c = CardDictionary.GetInGameCard(CardName.Innocence, p);
            b.SetCard(c);

            Assert.True(b.HasCardOf(p));
            Assert.Equal(c.Name, b.CardOf(c.Name).Name);
            Assert.Equal(c.Name, b.CardOf(p).Name);
            Assert.Equal(p, b.PlayerOf(c.Name));
        }

        [Fact]
        public void Test_DetectCard()
        {
            var b = GetBattle();
            Assert.False(b.DetectedCardName.HasValue);
            var cn = CardName.Innocence;
            b.DetectCard(cn);
            Assert.True(b.DetectedCardName.HasValue);
            Assert.Equal(cn, b.DetectedCardName);
        }

        [Fact]
        public void Test_DetectCard_ChooseNull()
        {
            var b = GetBattle();
            Assert.False(b.DetectedCardName.HasValue);
            b.DetectCard(null);
            Assert.False(b.DetectedCardName.HasValue);
        }

        [Fact]
        public void Test_JudgeWinnerByValues_AttackingPlayerWins()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Idol, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            Assert.Null(b.Winner);
            Assert.False(b.HasFinished());
            Assert.False(b.IsDrawBattle());

            b.Finish();
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
            var ac = CardDictionary.GetInGameCard(CardName.Doctor, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Idol, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            Assert.Null(b.Winner);
            Assert.False(b.HasFinished());
            Assert.False(b.IsDrawBattle());

            b.Finish();
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
            var ac = CardDictionary.GetInGameCard(CardName.Doctor, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            Assert.Null(b.Winner);
            Assert.False(b.HasFinished());
            Assert.False(b.IsDrawBattle());

            b.Finish();
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
            var ac = CardDictionary.GetInGameCard(CardName.Spy, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Trickster, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            b.SetToReverseCardValues();

            b.Finish();
            Assert.Equal(dp, b.Winner);
        }

        [Fact]
        public void Test_SetSpecialWinner_IgnoresCardValues()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Idol, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            b.SetSpecialWinner(dc.Name);
            Assert.Equal(dp, b.Winner);

            b.Finish();
            Assert.Equal(dp, b.Winner);
        }

        [Fact]
        public void Test_SetSpecialWinner_NotFinishUntilGameFinish()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Idol, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            b.SetSpecialWinner(dc.Name);
            Assert.False(b.HasFinished());

            b.Finish();
            Assert.True(b.HasFinished());
        }

        [Fact]
        public void Test_WiningStarOf_DefendingPlayerWinsWithNoDetection()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Creator, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            b.Finish();
            Assert.Equal(dp, b.Winner);

            Assert.Equal(0, b.WinningStarOf(ap));
            Assert.Equal(1, b.WinningStarOf(dp));
        }

        [Fact]
        public void Test_WiningStarOf_DefendingPlayerWinsAndDetectionFailed()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Creator, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            b.DetectCard(dc.Name);
            b.Finish();
            Assert.Equal(dp, b.Winner);

            Assert.Equal(0, b.WinningStarOf(ap));
            Assert.Equal(1, b.WinningStarOf(dp));
        }

        [Fact]
        public void Test_WiningStarOf_DefendingPlayerWinsAndDetectionSucceeded()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Creator, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            b.DetectCard(ac.Name);
            b.Finish();
            Assert.Equal(dp, b.Winner);

            Assert.Equal(0, b.WinningStarOf(ap));
            Assert.Equal(2, b.WinningStarOf(dp));
        }

        [Fact]
        public void Test_WiningStarOf_AttackingPlayerWinsAndDetectionIsCorrect()
        {
            var b = GetBattle();
            var ap = new PlayerAccount(attackingPlayerId);
            var dp = new PlayerAccount(defendingPlayerId);
            var ac = CardDictionary.GetInGameCard(CardName.Idol, ap);
            var dc = CardDictionary.GetInGameCard(CardName.Doctor, dp);

            b.SetCard(ac);
            b.SetCard(dc);
            b.DetectCard(ac.Name);
            b.Finish();
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
