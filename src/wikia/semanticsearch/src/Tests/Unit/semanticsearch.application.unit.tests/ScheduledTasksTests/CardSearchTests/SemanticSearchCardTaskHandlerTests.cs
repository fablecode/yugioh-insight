using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using semanticsearch.application.Configuration;
using semanticsearch.application.ScheduledTasks.CardSearch;
using semanticsearch.core.Search;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using semanticsearch.tests.core;

namespace semanticsearch.application.unit.tests.ScheduledTasksTests.CardSearchTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SemanticSearchCardTaskHandlerTests
    {
        private IOptions<AppSettings> _appSettingsOptions;
        private ISemanticSearchProcessor _semanticSearchProcessor;
        private SemanticSearchCardTaskHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _appSettingsOptions = Substitute.For<IOptions<AppSettings>>();
            _semanticSearchProcessor = Substitute.For<ISemanticSearchProcessor>();

            _sut = new SemanticSearchCardTaskHandler
            (
                Substitute.For<ILogger<SemanticSearchCardTaskHandler>>(),
                _appSettingsOptions, 
                _semanticSearchProcessor
            );
        }

        [Test]
        public async Task Given_A_SemanticSearchCardTask_Should_Process_It_Successfully()
        {
            // Arrange
            var semanticSearchCardTask = new SemanticSearchCardTask();
            _appSettingsOptions.Value.Returns(new AppSettings
            {
                CardSearchUrls = new Dictionary<string, string>
                {
                    ["normalmonsters"] = "https://www.youtube.com/"
                }
            });

            // Act
            var result = await _sut.Handle(semanticSearchCardTask, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}