using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Game;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class DoctorTest
    {
        [Fact]
        public void Test_WinsToHackerAndHackIsNotTriggered()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Hacker } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Hacker, CardName.Doctor, game);

            CustomAssertion.WinsInLastRound(CardName.Doctor, game);
            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));
            CustomAssertion.NotWaitingForCommand<HackCommand>(game);
            CustomAssertion.ActionIsNotInHistory<CommandStandbyEffect<HackCommand>>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToTrickster()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Trickster } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Trickster, CardName.Doctor, game);

            CustomAssertion.LosesInLastRound(CardName.Doctor, game);
            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));
            CustomAssertion.ActionIsNotInHistory<ReversalEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToSpyAndEspionageIsNotTriggered()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Spy } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Spy, CardName.Doctor, game);

            CustomAssertion.LosesInLastRound(CardName.Doctor, game);
            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));
            CustomAssertion.NotWaitingForCommand<EspionageCommand>(game);
            CustomAssertion.ActionIsNotInHistory<CommandStandbyEffect<EspionageCommand>>(game.ActionHistory);
        }

        [Fact]
        public void Test_NotMorphsAndLosesToBeast()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Beast } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Beast, CardName.Doctor, game);

            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));

            CustomAssertion.ActionIsNotInHistory<CommandStandbyEffect<MorphCommand>>(game.ActionHistory);
            CustomAssertion.NotWaitingForCommand<MorphCommand>(game);
            Assert.True(game.Battles[1].HasFinished());
            CustomAssertion.LosesInLastRound(CardName.Doctor, game);
        }

        [Fact]
        public void Test_LosesToLegionAndTrampleIsNotTriggered()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Legion } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Legion, CardName.Doctor, game);
            CustomAssertion.LosesInLastRound(CardName.Doctor, game);

            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));

            CustomAssertion.ActionIsNotInHistory<TrampleEffect>(game.ActionHistory);
            Assert.False(game.EffectIsDisabled(3, game.OverroidPlayer));
        }

        [Fact]
        public void Test_LosesToSoldierAndRushIsNotTriggered()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Soldier } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Soldier, CardName.Doctor, game);
            CustomAssertion.LosesInLastRound(CardName.Doctor, game);

            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));

            CustomAssertion.ActionIsNotInHistory<CommandStandbyEffect<RushCommand>>(game.ActionHistory);
            CustomAssertion.NotWaitingForCommand<RushCommand>(game);
        }

        [Fact]
        public void Test_LosesToOverroidButNoSingiularity()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Overroid, CardName.Doctor, game);
            CustomAssertion.LosesInLastRound(CardName.Doctor, game);

            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));

            CustomAssertion.ActionIsNotInHistory<SingularityEffect>(game.ActionHistory);
            Assert.False(game.HasFinished());
        }

        [Fact]
        public void Test_LosesToAttackingDeath()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Death } // Attacking
                );

            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Death, CardName.Doctor, game);
            CustomAssertion.LosesInLastRound(CardName.Doctor, game);

            CustomAssertion.ActionIsInHistory<JammingEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));

            CustomAssertion.ActionIsNotInHistory<SnipeEffect>(game.ActionHistory);
        }
    }
}
