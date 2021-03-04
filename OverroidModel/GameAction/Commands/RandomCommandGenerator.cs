namespace OverroidModel.GameAction.Commands
{
    public static class RandomCommandGenerator
    {
        public static IGameCommand CreateFromRequirement(ICommandRequirement requirement, IGameInformation game) => requirement switch
        {
            CommandRequirementImplement<CardPlacement> _ => CardPlacement.CreateRandomCommand(game, requirement.ComandingPlayer),
            CommandRequirementImplement<Detection> _ => Detection.CreateRandomCommand(game, requirement.ComandingPlayer),
            CommandRequirementImplement<HackCommand> _ => HackCommand.CreateRandomCommand(game, requirement.ComandingPlayer),
            CommandRequirementImplement<EspionageCommand> _ => EspionageCommand.CreateRandomCommand(game, requirement.ComandingPlayer),
            CommandRequirementImplement<MorphCommand> _ => MorphCommand.CreateRandomCommand(game, requirement.ComandingPlayer),
            CommandRequirementImplement<RushCommand> _ => RushCommand.CreateRandomCommand(game, requirement.ComandingPlayer),
            _ => throw new System.Exception($"Unexpected type command is expected: {requirement.CommandType}")
        };
    }
}
