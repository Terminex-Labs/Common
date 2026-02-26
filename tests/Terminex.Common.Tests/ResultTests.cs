using System.Net.WebSockets;
using Terminex.Common.Results;

namespace Terminex.Common.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Success_CreatesSuccessFullResult()
        {
            var result = Result.Success();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Failure_WithSingleError_CreatesFailedResult()
        {
            var error = new Error(ErrorCode.NotFound, "объект не найден!");

            var result = Result.Failure(error);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Single(result.Errors);
            Assert.Equal(error, result.Errors[0]);
        }

        [Fact]
        public void Failure_WithMultipleErrors_CreatesFailedResult()
        {
            var errors = new[]
            {
                new Error(ErrorCode.Validation, "Ошибка валидации 1"),
                new Error(ErrorCode.Validation, "Ошибка валидации 2")
            };

            var result = Result.Failure(errors);

            Assert.False(result.IsSuccess);
            Assert.Equal(2, result.Errors.Count);
        }

        [Fact]
        public void Match_OnSuccess_ExecutesCorrectBranch()
        {
            var result = Result.Success();
            var onSuccessCalled = false;

            var outPut = result.Match
                (
                    onSuccess: () => 
                    { 
                        onSuccessCalled = true; 
                        return "Ok"; 
                    },
                    onFailure: _ => "Fail"
                );

            Assert.True(onSuccessCalled);
            Assert.Equal("Ok", outPut);
        }

        [Fact]
        public void Match_OnFailure_ExecutesCorrectBranch()
        {
            var result = Result.Failure(new Error(ErrorCode.Null, "Ошибка!"));
            var onFailureCalled = false;

            var outPut = result.Match
                (
                    onSuccess: () => "Ok",
                    onFailure: errors => 
                    { 
                        onFailureCalled = true; 
                        return "Fail"; 
                    }
                );

            Assert.True(onFailureCalled);
            Assert.Equal("Fail", outPut);
        }

        [Fact]
        public void StringMessage_FormatsErrorsCorrectly()
        {
            var result = Result.Failure(new Error(ErrorCode.NotFound, "Не найдено!"));

            var message = result.StringMessage;

            Assert.Contains("Код: 6 - NotFound", message);
            Assert.Contains("Причина: Не найдено!", message);
        }
    }
}
