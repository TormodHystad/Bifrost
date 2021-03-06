using Bifrost.Commands;
using Bifrost.Domain;
using Bifrost.Events;
using Machine.Specifications;
using Moq;

namespace Bifrost.Specs.Domain.for_AggregatedRootRepository.given
{
    public class a_repository_for_a_stateless_aggregated_root
    {
        protected static AggregatedRootRepository<SimpleStatelessAggregatedRoot> repository;
        protected static Mock<ICommandContext> command_context_mock;
        protected static Mock<ICommandContextManager> command_context_manager_mock;
        protected static Mock<IEventStore> event_store_mock;

        Establish context = () =>
                                {
                                    command_context_mock = new Mock<ICommandContext>();
                                    command_context_manager_mock = new Mock<ICommandContextManager>();
                                    event_store_mock = new Mock<IEventStore>();
                                    command_context_mock.Setup(c => c.EventStores).Returns(new[] { event_store_mock.Object });
                                    repository = new AggregatedRootRepository<SimpleStatelessAggregatedRoot>(command_context_manager_mock.Object);
                                    command_context_manager_mock.Setup(ccm => ccm.GetCurrent()).Returns(command_context_mock.Object);
                                };
    }
}