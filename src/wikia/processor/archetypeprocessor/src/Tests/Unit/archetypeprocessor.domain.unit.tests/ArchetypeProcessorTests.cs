using archetypeprocessor.core.Models;
using archetypeprocessor.core.Models.Db;
using archetypeprocessor.core.Services;
using archetypeprocessor.domain.Messaging;
using archetypeprocessor.domain.Processor;
using archetypeprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace archetypeprocessor.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeProcessorTests
    {
        private IArchetypeService _archetypeService;
        private IImageQueueService _imageQueueService;
        private ArchetypeProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeService = Substitute.For<IArchetypeService>();
            _imageQueueService = Substitute.For<IImageQueueService>();

            _sut = new ArchetypeProcessor(_archetypeService, _imageQueueService);
        }

        [Test]
        public async Task Given_An_ArchetypeMessage_If_Archetype_Is_Not_Found_Should_Invoke_Add_Method()
        {
            // Arrange
            var archetypeMessage = new ArchetypeMessage();

            // Act
            await _sut.Process(archetypeMessage);

            // Assert
            await _archetypeService.Received(1).Add(Arg.Any<Archetype>());
        }

        [Test]
        public async Task Given_An_ArchetypeMessage_If_Archetype_Is_Not_Found_Should_Not_Invoke_Update_Method()
        {
            // Arrange
            var archetypeMessage = new ArchetypeMessage();

            // Act
            await _sut.Process(archetypeMessage);

            // Assert
            await _archetypeService.DidNotReceive().Update(Arg.Any<Archetype>());
        }

        [Test]
        public async Task Given_An_ArchetypeMessage_If_Archetype_Is_Found_Should_Invoke_Update_Method()
        {
            // Arrange
            var archetypeMessage = new ArchetypeMessage();
            _archetypeService.ArchetypeById(Arg.Any<long>()).Returns(new Archetype { Id = 42432, Name = "Blue-Eyes"});

            // Act
            await _sut.Process(archetypeMessage);

            // Assert
            await _archetypeService.Received(1).Update(Arg.Any<Archetype>());
        }

        [Test]
        public async Task Given_An_ArchetypeMessage_If_Archetype_Is_Found_Should_Not_Invoke_Add_Method()
        {
            // Arrange
            var archetypeMessage = new ArchetypeMessage();
            _archetypeService.ArchetypeById(Arg.Any<long>()).Returns(new Archetype { Id = 42432, Name = "Blue-Eyes"});

            // Act
            await _sut.Process(archetypeMessage);

            // Assert
            await _archetypeService.DidNotReceive().Add(Arg.Any<Archetype>());
        }

        [Test]
        public async Task Given_An_ArchetypeMessage_With_An_ImageUrl_Should_Invoke_Publish_Method()
        {
            // Arrange
            var archetypeMessage = new ArchetypeMessage
            {
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/3/3d/DMx001_Triple_Blue-Eyes.png/revision/latest/scale-to-width-down/350?cb=20140404164917"
            };

            _archetypeService.ArchetypeById(Arg.Any<long>()).Returns(new Archetype { Id = 42432, Name = "Blue-Eyes"});

            // Act
            await _sut.Process(archetypeMessage);

            // Assert
            await _imageQueueService.Received(1).Publish(Arg.Any<DownloadImage>());
        }
    }
}
