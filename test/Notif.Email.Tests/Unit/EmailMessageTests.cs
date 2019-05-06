using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Bogus;

namespace Notif.Email.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class EmailMessageTests
    {
        private readonly string _from;
        private readonly List<string> _toList;
        private readonly string _subject;
        private readonly string _body;

        public EmailMessageTests()
        {
            _from = "from@from.ru";
            _toList = new List<string>() { "to@to.ru" };
            _subject = "subject";
            _body = "body";
        }

        #region Ctor

        [Fact]
        public void Ctor_FromIsCorrect()
        {
            // Arrange
            // Act
            var message = new EmailMessage(_from, _toList, _subject, _body);
            // Assert
            Assert.Equal(_from, message.From);
        }

        [Fact]
        public void Ctor_ToIsCorrect()
        {
            // Arrange
            // Act
            var message = new EmailMessage(_from, _toList, _subject, _body);
            // Assert
            Assert.Equal(_toList.First(), message.To.First());
        }

        [Fact]
        public void Ctor_SubjectIsCorrect()
        {
            // Arrange
            // Act
            var message = new EmailMessage(_from, _toList, _subject, _body);
            // Assert
            Assert.Equal(_subject, message.Subject);
        }

        [Fact]
        public void Ctor_BodyIsCorrect()
        {
            // Arrange
            // Act
            var message = new EmailMessage(_from, _toList, _subject, _body);
            // Assert
            Assert.Equal(_body, message.Body);
        }

        #endregion
    }
}