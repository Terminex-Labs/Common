namespace Terminex.Common.Results
{
    public sealed record Error(ErrorCode ErrorCode, string Message)
    {
        public static Error New(ErrorCode errorCode) => new(errorCode, "");
        public static Error New(ErrorCode errorCode, string message) => new(errorCode, message);

        // Ошибки не найденного элемента
        public static Error Null(string message) => new(ErrorCode.Null, message);
        public static Error Empty(string message) => new(ErrorCode.Empty, message);

        public static Error Exist(string message) => new(ErrorCode.Exist, message);
        public static Error NotExist(string message) => new(ErrorCode.NotExist, message);
        public static Error NotFound(string message) => new(ErrorCode.NotFound, message);
        public static Error InvalidRequest(string message) => new(ErrorCode.InvalidRequest, message);
        public static Error InvalidResponse(string message) => new(ErrorCode.InvalidResponse, message);

        // Ошибки сервера
        public static Error Create(string message) => new(ErrorCode.Create, message);
        public static Error Update(string message) => new(ErrorCode.Update, message);
        public static Error Delete(string message) => new(ErrorCode.Delete, message);
        public static Error Save(string message) => new(ErrorCode.Save, message);
        public static Error Conflict(string message) => new(ErrorCode.Conflict, message);
        public static Error Connection(string message) => new(ErrorCode.Connection, message);
        public static Error Server(string message) => new(ErrorCode.Server, message);

        // Ошибки валидации
        public static Error BadRequest(string message) => new(ErrorCode.BadRequest, message);
        public static Error Validation(string message) => new(ErrorCode.Validation, message);
    }
}
