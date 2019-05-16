using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.RedirectType;
using Forte.RedirectTests.Builder.WithRepository;
using Forte.RedirectTests.Data;
using Forte.RedirectTests.RestExtensions;
using Xunit;

namespace Forte.RedirectTests.Tests
{
    public class ControllerTests
    {
        private static ControllerBuilder RedirectRuleController() => new ControllerBuilder();

        [Fact]
        public void Given_ExistingRedirects_Controller_ReturnsAllRedirects()
        {
            var rule1 = RandomDataGenerator.CreateRandomRedirectRule();
            var rule2 = RandomDataGenerator.CreateRandomRedirectRule();
            var existingRules = new HashSet<RedirectRule>
            {
                rule1,
                rule2
            };
            var dto1 = new RedirectRuleDto();
            var dto2 = new RedirectRuleDto();
            
            var restController = RedirectRuleController()
                .WithExplicitExistingRules(existingRules)
                .WithMapper(r => r == rule1 ? dto1 : r == rule2 ? dto2 : null)
                .Create();
            var resolvedRules = restController
                .Get()
                .GetEntitiesFromActionResult();
            
            Assert.Equal(new[] { dto1, dto2 }, resolvedRules);
        }

        [Fact]
        public void Given_ExistingRedirects_Controller_CreatesNewRedirect()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto("randomOldPath", "randomNewPath");

            var newRedirect = restController.Post(redirectDto).GetEntityFromActionResult();
            var expectedRedirect = restController.Get(newRedirect.Id.ExternalId).GetEntityFromActionResult();

            Assert.NotNull(expectedRedirect);
        }

        [Fact]
        public void Given_ExistingRedirects_Controller_UpdatesRedirect()
        {
            var rulesCount = 10;

            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .Create();

            var randomIndex = new Random().Next(rulesCount);
            var randomRedirectDto = restController
                .Get()
                .GetEntitiesFromActionResult()
                .Skip(randomIndex)
                .FirstOrDefault();

            var expectedNewUrl = "/updatedNewUrl";
            randomRedirectDto.NewPattern = expectedNewUrl;

            restController.Put(randomRedirectDto);
            var updatedRedirect = restController
                .Get(randomRedirectDto.Id.ExternalId)
                .GetEntityFromActionResult();

            Assert.Equal(expectedNewUrl, updatedRedirect?.NewPattern);
        }

        [Fact]
        public void Given_NotExistingRedirect_Controller_TriesToUpdateAndThrowsExceptionRedirectNotFound()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto(Guid.NewGuid(), "/NonExistentOldPath", "/randomNewUrl",
                RedirectType.Temporary);

            Assert.Throws<KeyNotFoundException>(() => restController.Put(redirectDto));
        }

        [Theory]
        [InlineData(true, HttpStatusCode.OK)]
        [InlineData(false, HttpStatusCode.Conflict)]
        public void Given_Redirects_Controller_ReturnsTrueIfFoundAndDeleted(bool doesExists, HttpStatusCode result)
        {
            var rulesCount = 10;
            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .Create();

            HttpStatusCode deleteResult;
            if (doesExists)
            {
                var randomIndex = new Random().Next(rulesCount);
                var randomRedirect = restController
                    .Get()
                    .GetEntitiesFromActionResult()
                    .Skip(randomIndex)
                    .FirstOrDefault();
                deleteResult = restController
                    .Delete(randomRedirect.Id.ExternalId)
                    .GetStatusCodeFromActionResult();
            }
            else
            {
                deleteResult = restController
                    .Delete(Guid.NewGuid())
                    .GetStatusCodeFromActionResult();
            }

            Assert.Equal(result, deleteResult);
        }
    }
}