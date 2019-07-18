using archetypedata.core.Models;
using archetypedata.domain.Processor;
using archetypedata.domain.Services.Messaging;
using archetypedata.domain.WebPages;
using archetypedata.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace archetypedata.domain.unit.tests.ArchetypeProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private IArchetypeWebPage _archetypeWebPage;
        private IQueue<Archetype> _queue;
        private ArchetypeProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeWebPage = Substitute.For<IArchetypeWebPage>();
            _queue = Substitute.For<IQueue<Archetype>>();

            _sut = new ArchetypeProcessor(_archetypeWebPage, _queue);
        }


    }
}
