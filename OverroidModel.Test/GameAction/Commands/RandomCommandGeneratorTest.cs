using OverroidModel.Card;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.GameAction.Commands
{
    public class RandomCommandGeneratorTest
    {
        [Fact]
        public void Test_CreateFromRequirement_CardPlacment()
        {
            var game = TestGameBuilder.CreateIndividualGame(round: 3);
            var requirement = new CommandRequirementImplement<CardPlacement>(game.OverroidPlayer);
            var command = RandomCommandGenerator.CreateFromRequirement(requirement, game);
            Assert.IsType<CardPlacement>(command);
        }

        [Fact]
        public void Test_CreateFromRequirement_Detection()
        {
            var game = TestGameBuilder.CreateIndividualGame(round: 3);
            var requirement = new CommandRequirementImplement<Detection>(game.OverroidPlayer);
            var command = RandomCommandGenerator.CreateFromRequirement(requirement, game);
            Assert.IsType<Detection>(command);
        }

        [Fact]
        public void Test_CreateFromRequirement_HackCommand()
        {
            var game = TestGameBuilder.CreateIndividualGame(round: 3);
            var requirement = new CommandRequirementImplement<HackCommand>(game.OverroidPlayer);
            var command = RandomCommandGenerator.CreateFromRequirement(requirement, game);
            Assert.IsType<HackCommand>(command);
        }

        [Fact]
        public void Test_CreateFromRequirement_EspionageCommand()
        {
            var game = TestGameBuilder.CreateIndividualGame(round: 3);
            var requirement = new CommandRequirementImplement<EspionageCommand>(game.OverroidPlayer);
            var command = RandomCommandGenerator.CreateFromRequirement(requirement, game);
            Assert.IsType<EspionageCommand>(command);
        }

        [Fact]
        public void Test_CreateFromRequirement_MorphCommand()
        {
            var game = TestGameBuilder.CreateIndividualGame(round: 3);
            var requirement = new CommandRequirementImplement<MorphCommand>(game.OverroidPlayer);
            var command = RandomCommandGenerator.CreateFromRequirement(requirement, game);
            Assert.IsType<MorphCommand>(command);
        }

        [Fact]
        public void Test_CreateFromRequirement_RushCommand()
        {
            var game = TestGameBuilder.CreateIndividualGame(round: 3);
            var requirement = new CommandRequirementImplement<RushCommand>(game.OverroidPlayer);
            var command = RandomCommandGenerator.CreateFromRequirement(requirement, game);
            Assert.IsType<RushCommand>(command);
        }
    }
}
