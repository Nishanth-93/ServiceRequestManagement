using ServiceRequestManagement.Domain.Seeds;
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
    }

    public class TestEntity : Entity
    {

    }
}
