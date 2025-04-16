using Application.Interfaces;
using BarterlyIntegrationTests.Core;
using Persistence.Database;

namespace BarterlyIntegrationTests.ServiceTests.General
{
    public class TokenServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly BarterlyDbContext _database;
        private readonly ITokenService _tokenService;

        public TokenServiceTests(DatabaseFixture fixture)
        {
            _database = fixture.dbContext;
            _tokenService = fixture.tokenService;
        }

        [Fact]
        public async Task GenerateAuthToken_ShouldReturnValidAuthToken()
        {
            // Arrange

            var userId = Guid.NewGuid();

            // Act 




            // Assert
        }
    }
}
