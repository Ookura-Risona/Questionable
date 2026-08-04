using Questionable.Controller.Steps.Common;
using Questionable.Controller.Steps.Interactions;
using Questionable.Model.Questing;
using Questionable.Tests.TestData;
using Xunit;

namespace Questionable.Tests.Steps;

public sealed class SnipeTests
{
    [Fact]
    public void SnipeStep_IsHandledAsInteractionTask()
    {
        Assert.True(Interact.IsInteractionTask(EInteractionType.Snipe));
        Assert.True(Interact.IsInteractionTask(EInteractionType.Interact));
        Assert.False(Interact.IsInteractionTask(EInteractionType.WaitForManualProgress));
    }

    [Fact]
    public void SnipeStep_DoesNotCreateManualNotification()
    {
        var factory = new SendNotification.Factory(null!, null!, null!, null!);
        var (quest, sequence, step) = QuestTestData.FactoryContext(
            new QuestId(1),
            1,
            new QuestStep { InteractionType = EInteractionType.Snipe, DataId = 123 });

        Assert.Null(factory.CreateTask(quest, sequence, step));
    }
}
