using System.Diagnostics;
using Terminex.Common.Results;
using Xunit.Abstractions;

namespace Terminex.Common.Tests
{
    public class ResultTypeTests(ITestOutputHelper output)
    {
        private readonly ITestOutputHelper _output = output;

        [Fact]
        public void Success_WithValidValue_CreatesSuccessFullResult()
        {
            var result = Result<string>.Success("Данные");

            Assert.True(result.IsSuccess);
            Assert.Equal("Данные", result.Value);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Value_WhenFailure_ThrowsInvalidOperationException()
        {
            var result = Result<int>.Failure(new Error(ErrorCode.None, "Ошибка"));

            var exception = Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.Equal("Нельзя получить значение из неуспешного результата.", exception.Message);
        }

        [Fact]
        public void Success_WithNullValue_ForReferenceType_Allowed()
        {
            var result = Result<string>.Success(null!);

            Assert.True(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Match_TypedResult_OnSuccess_ExecutesCorrectBranch()
        {
            var result = Result<int>.Success(42);

            var outPut = result.Match
                (
                    onSuccess: value => value * 2,
                    onFailure: _ => -1
                );

            Assert.Equal(84, outPut);
        }

        [Fact]
        public void Switch_TypedResult_OnFailure_ExecutesCorrectBranch()
        {
            var result = Result<string>.Failure(new Error(ErrorCode.Validation, "Ошибка валидации"));
            var executed = false;

            result.Switch
                (
                    onSuccess: _ => executed = true,
                    onFailure: errors => executed = errors.Count > 0
                );

            Assert.True(executed);
        }

        [Fact]
        public void Failure_InheritsBaseBehavior()
        {
            var error = new Error(ErrorCode.NotFound, "Не найдено");

            var result = Result<double>.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.Single(result.Errors);
            Assert.Contains("Не найдено", result.StringMessage);
        }
    }
}
