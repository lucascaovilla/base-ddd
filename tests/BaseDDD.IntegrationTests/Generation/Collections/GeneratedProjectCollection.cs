using Xunit;
using BaseDDD.IntegrationTests.Generation.Fixtures;

namespace BaseDDD.IntegrationTests.Generation.Collections;

[CollectionDefinition("GeneratedProject", DisableParallelization = true)]
public class GeneratedProjectCollection : ICollectionFixture<GeneratedProjectFixture>
{
}
