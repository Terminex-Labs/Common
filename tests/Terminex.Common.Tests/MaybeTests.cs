using Terminex.Common.Primitives;

namespace Terminex.Common.Tests
{
    public class MaybeTests
    {
        #region Construction & Basic Properties

        [Fact]
        public void None_ShouldHaveHasValueFalse()
        {
            // Arrange & Act
            var maybe = Maybe<int>.None;

            // Assert
            Assert.True(maybe.IsNone);
            Assert.False(maybe.HasValue);
        }

        [Fact]
        public void Some_WithValue_ShouldHaveHasValueTrue()
        {
            // Arrange & Act
            var maybe = Maybe<int>.Some(42);

            // Assert
            Assert.True(maybe.HasValue);
            Assert.False(maybe.IsNone);
        }

        [Fact]
        public void Some_WithNullReferenceType_ShouldReturnNone()
        {
            // Arrange
            string? nullValue = null;

            // Act
            var maybe = Maybe<string>.Some(nullValue);

            // Assert
            Assert.True(maybe.IsNone);
            Assert.False(maybe.HasValue);
        }

        [Fact]
        public void Some_WithNonNullReferenceType_ShouldReturnValue()
        {
            // Arrange
            var expected = "test";

            // Act
            var maybe = Maybe<string>.Some(expected);

            // Assert
            Assert.True(maybe.HasValue);
            Assert.Equal(expected, maybe.Value);
        }

        [Fact]
        public void Some_WithDefaultValueType_ShouldReturnValue()
        {
            // Act
            var maybe = Maybe<int>.Some(0); // 0 — валидное значение для int

            // Assert
            Assert.True(maybe.HasValue);
            Assert.Equal(0, maybe.Value);
        }

        #endregion

        #region Value Property

        [Fact]
        public void Value_WhenHasValue_ShouldReturnWrappedValue()
        {
            // Arrange
            var expected = "hello";
            var maybe = Maybe<string>.Some(expected);

            // Act
            var actual = maybe.Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Value_WhenIsNone_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var maybe = Maybe<string>.None;

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => maybe.Value);
            Assert.Equal("Не удалось получить значение", exception.Message);
        }

        #endregion

        #region GetValueOrDefault

        [Fact]
        public void GetValueOrDefault_WhenHasValue_ShouldReturnWrappedValue()
        {
            // Arrange
            var maybe = Maybe<int>.Some(100);

            // Act
            var result = maybe.GetValueOrDefault(999);

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public void GetValueOrDefault_WhenIsNone_ShouldReturnDefaultValue()
        {
            // Arrange
            var maybe = Maybe<int>.None;

            // Act
            var result = maybe.GetValueOrDefault(999);

            // Assert
            Assert.Equal(999, result);
        }

        [Fact]
        public void GetValueOrDefault_WithDefaultParameter_WhenIsNone_ShouldReturnDefaultOfType()
        {
            // Arrange
            var maybe = Maybe<string>.None;

            // Act
            var result = maybe.GetValueOrDefault();

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Match (Action)

        [Fact]
        public void Match_Action_WhenHasValue_ShouldInvokeOnSome()
        {
            // Arrange
            var maybe = Maybe<int>.Some(42);
            var onSomeCalled = false;
            var onNoneCalled = false;

            // Act
            maybe.Match(
                onSome: value => { onSomeCalled = true; Assert.Equal(42, value); },
                onNone: () => onNoneCalled = true
            );

            // Assert
            Assert.True(onSomeCalled);
            Assert.False(onNoneCalled);
        }

        [Fact]
        public void Match_Action_WhenIsNone_ShouldInvokeOnNone()
        {
            // Arrange
            var maybe = Maybe<int>.None;
            var onSomeCalled = false;
            var onNoneCalled = false;

            // Act
            maybe.Match
                (
                    onSome: _ => onSomeCalled = true,
                    onNone: () => onNoneCalled = true
                );

            // Assert
            Assert.False(onSomeCalled);
            Assert.True(onNoneCalled);
        }

        #endregion

        #region Match (Func<TResult>)

        [Fact]
        public void Match_Func_WhenHasValue_ShouldInvokeOnSomeAndReturnResult()
        {
            // Arrange
            var maybe = Maybe<int>.Some(5);

            // Act
            var result = maybe.Match
                (
                    onSome: value => value * 2,
                    onNone: () => -1
                );

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Match_Func_WhenIsNone_ShouldInvokeOnNoneAndReturnResult()
        {
            // Arrange
            var maybe = Maybe<int>.None;

            // Act
            var result = maybe.Match
                (
                    onSome: value => value * 2,
                    onNone: () => -1
                );

            // Assert
            Assert.Equal(-1, result);
        }

        #endregion

        #region Map

        [Fact]
        public void Map_WhenHasValue_ShouldApplyFunctionAndReturnSome()
        {
            // Arrange
            var maybe = Maybe<int>.Some(10);

            // Act
            var result = maybe.Map(x => x.ToString());

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal("10", result.Value);
        }

        [Fact]
        public void Map_WhenIsNone_ShouldReturnNone()
        {
            // Arrange
            var maybe = Maybe<int>.None;

            // Act
            var result = maybe.Map(x => x.ToString());

            // Assert
            Assert.True(result.IsNone);
        }

        [Fact]
        public void Map_ChainedOperations_ShouldWorkCorrectly()
        {
            // Arrange
            var maybe = Maybe<int>.Some(5);

            // Act
            var result = maybe
                .Map(x => x * 2)      // 10
                .Map(x => x + 3)      // 13
                .Map(x => x / 13);    // 1

            // Assert
            Assert.Equal(1, result.Value);
        }

        #endregion

        #region Bind

        [Fact]
        public void Bind_WhenHasValue_ShouldApplyFunctionAndReturnResult()
        {
            // Arrange
            var maybe = Maybe<int>.Some(10);

            // Act
            var result = maybe.Bind(x => Maybe<string>.Some($"Value: {x}"));

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal("Value: 10", result.Value);
        }

        [Fact]
        public void Bind_WhenIsNone_ShouldReturnNoneWithoutInvokingFunction()
        {
            // Arrange
            var maybe = Maybe<int>.None;
            var functionCalled = false;

            // Act
            var result = maybe.Bind
                (
                    _ =>
                    {
                        functionCalled = true;
                        return Maybe<string>.Some("test");
                    }
                );

            // Assert
            Assert.False(functionCalled);
            Assert.True(result.IsNone);
        }

        [Fact]
        public void Bind_ChainedOperations_WithIntermediateNone_ShouldShortCircuit()
        {
            // Arrange
            var maybe = Maybe<int>.Some(5);

            // Act
            var result = maybe
                .Bind(x => Maybe<int>.Some(x * 2))    // Some(10)
                .Bind(_ => Maybe<int>.None)           // None
                .Bind(x => Maybe<int>.Some(x + 1));   // Не выполнится

            // Assert
            Assert.True(result.IsNone);
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_WhenHasValue_ShouldReturnSomeFormat()
        {
            // Arrange
            var maybe = Maybe<int>.Some(42);

            // Act
            var result = maybe.ToString();

            // Assert
            Assert.Equal("Some(42)", result);
        }

        [Fact]
        public void ToString_WhenIsNone_ShouldReturnNone()
        {
            // Arrange
            var maybe = Maybe<string>.None;

            // Act
            var result = maybe.ToString();

            // Assert
            Assert.Equal("None", result);
        }

        [Fact]
        public void ToString_WithReferenceTypeValue_ShouldIncludeValue()
        {
            // Arrange
            var maybe = Maybe<string>.Some("hello");

            // Act
            var result = maybe.ToString();

            // Assert
            Assert.Equal("Some(hello)", result);
        }

        #endregion

        #region Implicit Operator

        [Fact]
        public void ImplicitConversion_FromValue_ShouldCreateSome()
        {
            // Act
            Maybe<int> maybe = 42;

            // Assert
            Assert.True(maybe.HasValue);
            Assert.Equal(42, maybe.Value);
        }

        [Fact]
        public void ImplicitConversion_FromNullReference_ShouldCreateNone()
        {
            // Act
            Maybe<string> maybe = (string?)null;

            // Assert
            Assert.True(maybe.IsNone);
        }

        [Fact]
        public void ImplicitConversion_FromNonNullReference_ShouldCreateSome()
        {
            // Act
            Maybe<string> maybe = "test";

            // Assert
            Assert.True(maybe.HasValue);
            Assert.Equal("test", maybe.Value);
        }

        #endregion

        #region Record Struct Behavior (Equality)

        [Fact]
        public void Equality_TwoNoneInstances_ShouldBeEqual()
        {
            // Arrange
            var maybe1 = Maybe<int>.None;
            var maybe2 = Maybe<int>.None;

            // Act & Assert
            Assert.Equal(maybe1, maybe2);
            Assert.True(maybe1 == maybe2);
        }

        [Fact]
        public void Equality_TwoSomeWithSameValue_ShouldBeEqual()
        {
            // Arrange
            var maybe1 = Maybe<int>.Some(42);
            var maybe2 = Maybe<int>.Some(42);

            // Act & Assert
            Assert.Equal(maybe1, maybe2);
            Assert.True(maybe1 == maybe2);
        }

        [Fact]
        public void Equality_SomeAndNone_ShouldNotBeEqual()
        {
            // Arrange
            var maybe1 = Maybe<int>.Some(42);
            var maybe2 = Maybe<int>.None;

            // Act & Assert
            Assert.NotEqual(maybe1, maybe2);
            Assert.True(maybe1 != maybe2);
        }

        [Fact]
        public void Equality_TwoSomeWithDifferentValues_ShouldNotBeEqual()
        {
            // Arrange
            var maybe1 = Maybe<int>.Some(42);
            var maybe2 = Maybe<int>.Some(43);

            // Act & Assert
            Assert.NotEqual(maybe1, maybe2);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Some_WithEmptyString_ShouldCreateSome()
        {
            // Arrange
            var empty = string.Empty;

            // Act
            var maybe = Maybe<string>.Some(empty);

            // Assert
            Assert.True(maybe.HasValue);
            Assert.Equal(string.Empty, maybe.Value);
        }

        [Fact]
        public void Map_WithNullResult_ShouldHandleCorrectly()
        {
            // Arrange
            var maybe = Maybe<int>.Some(42);

            // Act
            var result = maybe.Map(_ => (string?)null);

            // Assert
            Assert.True(result.IsNone); // Some(null) должен стать None
        }

        [Fact]
        public void GetValueOrDefault_WithValueTypeDefault_ShouldWork()
        {
            // Arrange
            var maybe = Maybe<int>.None;

            // Act
            var result = maybe.GetValueOrDefault(-1);

            // Assert
            Assert.Equal(-1, result);
        }

        #endregion
    }
}
