using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using Ensek.Domain;
using Ensek.Domain.Validators;
using FluentAssertions;
using Ensek.Domain.Data.Domain;

namespace Ensek.Tests.Domain.Validators
{

    public class GivenAnAccountUpdateValidator
    {
        private static AccountUpdateValidator Validator => new();

        public static AccountUpdate ValidAccountUpdate => new()
        {
            AccountId = 10,
            Firstname = "FTest",
            Surname = "STest",
            ImporterId = Guid.NewGuid(),
        };

        [Test]
        public void It_can_be_constructed()
        {
            Validator.Should().NotBeNull();
        }

        [Test]
        public void It_passes_valid_account_updates()
        {
            // arrange 


            // act 
            var result = Validator.Validate(ValidAccountUpdate);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Errors.Should().HaveCount(0);
        }

        [Test]
        public void It_fails_incorrect_accounts()
        {
            // arrange 
            var v = ValidAccountUpdate;
            v.AccountId = -1;

            // act 
            var result = Validator.Validate(v);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void It_fails_incorrect_surname()
        {
            // arrange 
            var v = ValidAccountUpdate;
            v.Surname = string.Empty;

            // act 
            var result = Validator.Validate(v);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void It_fails_incorrect_firstname()
        {
            // arrange 
            var v = ValidAccountUpdate;
            v.Firstname = string.Empty;

            // act 
            var result = Validator.Validate(v);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void It_fails_incorrect_ImporterId()
        {
            // arrange 
            var v = ValidAccountUpdate;
            v.ImporterId = Guid.Empty;

            // act 
            var result = Validator.Validate(v);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
        }

        // -----------  helpers -----------



    }
}