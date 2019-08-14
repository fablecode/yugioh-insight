|**Applications**|**Description**
|:-----:|:-----:|
[Article](https://github.com/fablecode/yugioh-insight/tree/master/src/wikia/article)| Articles to process
[Data](https://github.com/fablecode/yugioh-insight/tree/master/src/wikia/data)| Amalgamate article data
[Processor](https://github.com/fablecode/yugioh-insight/tree/master/src/wikia/processor)| Persist article

# Yugioh-Insight
Yugioh Insight is a collection of solutions for gathering [Yu-Gi-Oh](http://www.yugioh-card.com/uk/) data from various sources.

## Built With
* [Visual Studio 2019](https://www.visualstudio.com/downloads/)
* [.NET Core 2.2](https://www.microsoft.com/net/download/core)
* [Onion Architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/) and [CQRS](https://martinfowler.com/bliki/CQRS.html).
* [RabbitMq](https://www.rabbitmq.com/)
* [Visual Studio Team Services](https://www.visualstudio.com/team-services/release-management/) for CI and deployment.
* [Dataflow Blocks (Task Parallel Library)](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library)
* [Mediatr](https://www.nuget.org/packages/MediatR/) for CQRS and the Mediator Design Pattern. Mediator design pattern defines how a set of objects interact with each other. You can think of a Mediator object as a kind of traffic-coordinator, it directs traffic to appropriate parties.
* [Fluent Validations](https://www.nuget.org/packages/FluentValidation)
* [Fluent Assertions](https://www.nuget.org/packages/FluentAssertions)
* [NUnit](https://github.com/nunit/nunit)
* [Scrutor](https://github.com/khellang/Scrutor) for decorator pattern implementation
* [Dataflow Blocks (Task Parallel Library)](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library)