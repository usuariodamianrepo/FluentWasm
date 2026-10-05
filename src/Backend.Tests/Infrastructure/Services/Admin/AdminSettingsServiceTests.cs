namespace Backend.Tests.Infrastructure.Services.Admin
{
    using Backend.Application.DTOs;
    using Backend.Application.DTOs.Admin.Audit;
    using Backend.Application.DTOs.Admin.Settings;
    using Backend.Application.Services.Contracts.Admin;
    using Backend.Infrastructure.Data;
    using Backend.Infrastructure.Services.Admin;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class AdminSettingsServiceTests
    {
        [Fact]
        public async Task UpdateStoreAsync_RejectsInvalidCurrency()
        {
            await using var context = CreateContext();
            var service = CreateService(context);

            var result = await service.UpdateAppAsync(new UpdateAppSettingsDto
            {
                AppName = "MyApp",
                DefaultCurrency = "DOLARS",
                DefaultCulture = "en-US",
            });

            Assert.False(result.Success);
            Assert.Equal(ServiceResponseType.ValidationError, result.ResponseType);
        }

        private static AdminSettingsService CreateService(AppDbContext context)
        {
            var audit = new Mock<IAdminAuditService>();
            audit.Setup(service => service.LogAsync(It.IsAny<CreateAdminAuditLogDto>()))
                .ReturnsAsync(new ServiceResponse<AdminAuditLogDto>(true)
                {
                    Payload = new AdminAuditLogDto { Id = Guid.NewGuid() },
                    ResponseType = ServiceResponseType.Success,
                });

            return new AdminSettingsService(
                context,
                Options.Create(new EmailSettings()),
                new TestHostEnvironment(),
                new HttpContextAccessor(),
                audit.Object);
        }

        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"admin-settings-{Guid.NewGuid()}")
                .Options;

            return new AppDbContext(options);
        }

        private sealed class TestHostEnvironment : IHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Testing";

            public string ApplicationName { get; set; } = "MyApp.Tests";

            public string ContentRootPath { get; set; } = AppContext.BaseDirectory;

            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        }
    }
}
