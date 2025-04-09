/*using Moq;
using Xunit;
using Atsi.Structures.PKB.Explorer;
using Atsi.Structures.SIMPLE;
using System.Collections.Generic;
using System.Linq;

namespace Atsi.Structures.Tests.PKB.Explorer
{
    public class PKBQueryServiceTests
    {
        private readonly Mock<PKBStorage> _mockStorage;
        private readonly PKBQueryService _service;

        public PKBQueryServiceTests()
        {
            _mockStorage = new Mock<PKBStorage>();
            _service = new PKBQueryService(_mockStorage.Object);
        }

        // === Tests for Procedure ===
        [Fact]
        public void GetProcedure_ShouldReturnProcedure_WhenExists()
        {
            // Arrange
            var expected = new Procedure("test");
            _mockStorage.Setup(x => x.GetProcedure("test")).Returns(expected);

            // Act
            var result = _service.GetProcedure("test");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAllProcedureNames_ShouldReturnAllNames()
        {
            // Arrange
            var procedures = new Dictionary<string, Procedure>
            {
                {"proc1", new Procedure("proc1")},
                {"proc2", new Procedure("proc2")}
            };
            _mockStorage.Setup(x => x.Procedures).Returns(procedures);

            // Act
            var result = _service.GetAllProcedureNames();

            // Assert
            Assert.Equal(new[] {"proc1", "proc2"}, result.OrderBy(x => x));
        }

        // === Tests for Follows ===
        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(1, 3, false)]
        public void IsFollows_ShouldReturnCorrectValue(int stmt1, int stmt2, bool expected)
        {
            // Arrange
            _mockStorage.Setup(x => x.IsFollows(stmt1, stmt2)).Returns(expected);

            // Act & Assert
            Assert.Equal(expected, _service.IsFollows(stmt1, stmt2));
        }

        [Fact]
        public void GetFollows_ShouldReturnNextStatement()
        {
            // Arrange
            var follows = new Dictionary<int, int> { {1, 2}, {2, 3} };
            _mockStorage.Setup(x => x.Follows).Returns(follows);

            // Act & Assert
            Assert.Equal(2, _service.GetFollows(1));
            Assert.Equal(3, _service.GetFollows(2));
            Assert.Null(_service.GetFollows(3));
        }

        [Fact]
        public void IsFollowsTransitive_ShouldDetectTransitiveRelations()
        {
            // Arrange
            var follows = new Dictionary<int, int> { {1, 2}, {2, 3}, {3, 4} };
            _mockStorage.Setup(x => x.Follows).Returns(follows);

            // Act & Assert
            Assert.True(_service.IsFollowsTransitive(1, 3));
            Assert.True(_service.IsFollowsTransitive(1, 4));
            Assert.False(_service.IsFollowsTransitive(2, 1));
            Assert.False(_service.IsFollowsTransitive(1, 5));
        }

        // === Tests for Parent ===
        [Fact]
        public void IsParent_ShouldReturnCorrectValue()
        {
            // Arrange
            _mockStorage.Setup(x => x.IsParent(1, 2)).Returns(true);
            _mockStorage.Setup(x => x.IsParent(1, 3)).Returns(false);

            // Act & Assert
            Assert.True(_service.IsParent(1, 2));
            Assert.False(_service.IsParent(1, 3));
        }

        [Fact]
        public void GetAllNestedStatements_ShouldReturnAllDescendants()
        {
            // Arrange
            var parent = new Dictionary<int, int> { {2, 1}, {3, 1}, {4, 2}, {5, 2} };
            _mockStorage.Setup(x => x.Parent).Returns(parent);

            // Act
            var result = _service.GetAllNestedStatements(1);

            // Assert
            Assert.Equal(new[] {2, 3, 4, 5}, result.OrderBy(x => x));
        }

        // === Tests for Modifies ===
        [Fact]
        public void IsModifies_ShouldCheckVariableCorrectly()
        {
            // Arrange
            var modifies = new Dictionary<int, HashSet<string>>
            {
                {1, new HashSet<string> {"x", "y"}},
                {2, new HashSet<string> {"z"}}
            };
            _mockStorage.Setup(x => x.Modifies).Returns(modifies);

            // Act & Assert
            Assert.True(_service.IsModifies(1, "x"));
            Assert.True(_service.IsModifies(1, "y"));
            Assert.False(_service.IsModifies(1, "z"));
            Assert.False(_service.IsModifies(3, "x"));
        }

        // === Tests for Uses ===
        [Fact]
        public void GetStatementsUsing_ShouldReturnCorrectStatements()
        {
            // Arrange
            var uses = new Dictionary<int, HashSet<string>>
            {
                {1, new HashSet<string> {"x", "y"}},
                {2, new HashSet<string> {"x"}},
                {3, new HashSet<string> {"z"}}
            };
            _mockStorage.Setup(x => x.Uses).Returns(uses);

            // Act
            var result = _service.GetStatementsUsing("x");

            // Assert
            Assert.Equal(new[] {1, 2}, result.OrderBy(x => x));
        }

        [Fact]
        public void GetAllUsedVariables_ShouldReturnDistinctVariables()
        {
            // Arrange
            var uses = new Dictionary<int, HashSet<string>>
            {
                {1, new HashSet<string> {"x", "y"}},
                {2, new HashSet<string> {"x", "z"}}
            };
            _mockStorage.Setup(x => x.Uses).Returns(uses);

            // Act
            var result = _service.GetAllUsedVariables();

            // Assert
            Assert.Equal(new[] {"x", "y", "z"}, result.OrderBy(x => x));
        }

        // === Additional edge case tests ===
        [Fact]
        public void GetAllParentPairs_ShouldHandleEmptyCollection()
        {
            // Arrange
            _mockStorage.Setup(x => x.Parent).Returns(new Dictionary<int, int>());

            // Act
            var result = _service.GetAllParentPairs();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetModifiedVariables_ShouldReturnEmptyForUnknownStatement()
        {
            // Arrange
            _mockStorage.Setup(x => x.Modifies).Returns(new Dictionary<int, HashSet<string>>());

            // Act
            var result = _service.GetModifiedVariables(99);

            // Assert
            Assert.Empty(result);
        }
    }
}*/