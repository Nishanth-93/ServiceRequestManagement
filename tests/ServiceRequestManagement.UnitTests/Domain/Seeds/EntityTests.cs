using MediatR;
using ServiceRequestManagement.Domain.Seeds;
using System;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Domain.Seeds
{
    public class EntityTests
    {
        [Fact]
        public void Given_TransientEntity_When_CallingIsTransient_Then_ReturnsTrue()
        {
            // Arrange
            var testEntity = new TestEntity();

            // Act
            var actual = testEntity.IsTransient();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Given_TwoTransientEntities_When_ComparingEquality_Then_ReturnsFalse()
        {
            // Arrange
            var leftEntity = new TestEntity();
            var rightEntity = new TestEntity();

            // Act
            var actual = leftEntity.Equals(rightEntity);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Given_TwoTransientEntities_When_ComparingEquality_WithEqualityOperator_Then_ReturnsFalse()
        {
            // Arrange
            var leftEntity = new TestEntity();
            var rightEntity = new TestEntity();

            // Act
            var actual = leftEntity == rightEntity;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Given_TwoTransientEntities_When_ComparingInequality_WithInequalityOperator_Then_ReturnsTrue()
        {
            // Arrange
            var leftEntity = new TestEntity();
            var rightEntity = new TestEntity();

            // Act
            var actual = leftEntity != rightEntity;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Given_Id_When_ConstructingTestEntity_Then_SetsIdCorrectly()
        {
            // Arrange
            var expectedId = Guid.NewGuid();

            // Act
            var actual = new TestEntity(expectedId);

            // Assert
            Assert.Equal(expectedId, actual.Id);
        }

        [Fact]
        public void Given_TestNotifications_When_AddingNotifications_Then_NotificationsAreSucessfullyAdded()
        {
            // Arrange
            var target = new TestEntity();
            var expectedNotification1 = new TestNotification { Id = Guid.NewGuid() };
            var expectedNotification2 = new TestNotification { Id = Guid.NewGuid() };

            // Act
            target.AddDomainEvents(expectedNotification1);
            target.AddDomainEvents(expectedNotification2);

            // Assert
            Assert.Collection(target.DomainEvents,
                evnt =>
                {
                    Assert.Equal(evnt, expectedNotification1);
                },
                evnt =>
                {
                    Assert.Equal(evnt, expectedNotification2);
                });
        }

        [Fact]
        public void Given_TestNotifications_With_EventsInDomainEvents_When_RemovingNotifications_Then_NotificationsAreSucessfullyRemoved()
        {
            // Arrange
            var target = new TestEntity();
            var expectedNotification = new TestNotification { Id = Guid.NewGuid() };
            target.AddDomainEvents(expectedNotification);

            // Act
            target.RemoveDomainEvent(expectedNotification);
            target.RemoveDomainEvent(expectedNotification);

            // Assert
            Assert.Empty(target.DomainEvents);
        }

        [Fact]
        public void Given_TestNotifications_With_EventsInDomainEvents_When_ClearingNotifications_Then_NotificationsAreSucessfullyCleared()
        {
            // Arrange
            var target = new TestEntity();
            target.AddDomainEvents(new TestNotification { Id = Guid.NewGuid() });

            // Act
            target.ClearDomainEvents();

            // Assert
            Assert.Empty(target.DomainEvents);
        }

        [Fact]
        public void Given_TestNotifications_With_NoEventsInDomainEvents_When_ClearingNotifications_Then_NullReferenceExceptionNotThrown()
        {
            // Arrange
            var target = new TestEntity();
            var notification = new TestNotification { Id = Guid.NewGuid() };
            target.AddDomainEvents(notification);
            target.RemoveDomainEvent(notification);

            // Act
            target.ClearDomainEvents();

            // Assert
            Assert.Empty(target.DomainEvents);
        }

        [Fact]
        public void Given_TwoNotifications_With_DifferentIds_When_ComparingEquality_Then_ReturnFalse()
        {
            // Arrange
            var left = new TestEntity(Guid.NewGuid());
            var right = new TestEntity(Guid.NewGuid());

            // Act
            var actual = left.Equals(right);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Given_TwoNotifications_With_SameIds_When_ComparingEquality_Then_ReturnTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var left = new TestEntity(id);
            var right = new TestEntity(id);

            // Act
            var actual = left.Equals(right);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Given_EntityNotifications_And_TransientNotification_When_ComparingEquality_Then_ReturnFalse()
        {
            // Arrange
            var left = new TestEntity(Guid.NewGuid());
            var right = new TestEntity();

            // Act
            var actual = left.Equals(right);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Given_NullNotifications_And_NonNullNotification_When_ComparingOperatorEquality_Then_ReturnFalse()
        {
            // Arrange
            var left = (TestEntity)null;
            var right = new TestEntity(Guid.NewGuid());

            // Act
            var actual = left == right;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Given_TwoNullNotifications_When_ComparingOperatorEquality_Then_ReturnTrue()
        {
            // Arrange
            var left = (TestEntity)null;
            var right = (TestEntity)null;

            // Act
            var actual = left == right;

            // Assert
            Assert.True(actual);
        }
    }

    public class TestEntity : Entity
    {
        public TestEntity()
        {
            Id = default;
        }

        public TestEntity(Guid id)
        {
            Id = id;
        }
    }

    public class TestNotification : INotification
    {
        public Guid Id { get; set; }
    }
}
