namespace Terminex.Common.Results
{
    public interface IResultWithFactory<TResult> where TResult : Result
    {
        public static abstract TResult CreateFailure(IEnumerable<Error> errors);
    }
}