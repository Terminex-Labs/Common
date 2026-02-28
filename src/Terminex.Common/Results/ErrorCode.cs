namespace Terminex.Common.Results
{
    //public enum ErrorCode
    //{
    //    None = 0,
    //    Create = 1,
    //    Delete = 2,
    //    Update = 3,
    //    NotFound = 4,
    //    InvalidPassword = 5,
    //    Server = 6,
    //    Conflict = 7,
    //    Save = 8,
    //    Unauthorized = 9,
    //    Validation = 10,
    //    ValueObject = 11,
    //    Redis = 12
    //}

    public readonly record struct ErrorCode : IEquatable<ErrorCode>
    {
        public readonly string Name { get; }
        public readonly int Code { get; }

        private ErrorCode(string name, int code)
        {
            Name = name;
            Code = code;
        }

        // Ошибки не найденного элемента
        public static readonly ErrorCode None = new(nameof(None), 1);
        public static readonly ErrorCode Null = new(nameof(Null), 2);
        public static readonly ErrorCode Empty = new(nameof(Empty), 3);

        public static readonly ErrorCode Exist = new(nameof(Exist), 4);
        public static readonly ErrorCode NotExist = new(nameof(NotExist), 5);
        public static readonly ErrorCode NotFound = new(nameof(NotFound), 6);
        public static readonly ErrorCode InvalidRequest = new(nameof(InvalidRequest), 7);
        public static readonly ErrorCode InvalidResponse = new(nameof(InvalidResponse), 8);

        // Серверные ошибки
        public static readonly ErrorCode Create = new(nameof(Create), 9);
        public static readonly ErrorCode Update = new(nameof(Update), 10);
        public static readonly ErrorCode Delete = new(nameof(Delete), 1);
        public static readonly ErrorCode Save = new(nameof(Save), 12);
        public static readonly ErrorCode Conflict = new(nameof(Conflict), 13);
        public static readonly ErrorCode Connection = new(nameof(Connection), 14);
        public static readonly ErrorCode Server = new(nameof(Server), 15);

        /// <summary>
        /// Не корректный синтаксис или не праавильные параметры запроса
        /// </summary>
        public static readonly ErrorCode BadRequest = new(nameof(BadRequest), 16);

        // Валидационные ошибки
        public static readonly ErrorCode Validation = new(nameof(Validation), 17);

        // Ошибки аутентификации и авторизации

        /// <summary>
        /// Аутентификация не пройдена, неверный пароль/логин, истекший или недействительный токен
        /// </summary>
        public static readonly ErrorCode Unauthorized = new(nameof(Unauthorized), 18);

        /// <summary>
        /// Пользователь аутентифицирован, но не имеет прав доступа к данному ресурсу (запрещено)
        /// </summary>
        public static readonly ErrorCode Forbidden = new(nameof(Forbidden), 19);

        public static ErrorCode Custom(string name, int code)
        {
            if (code < 10000 && code > 29999)
                throw new ArgumentOutOfRangeException(nameof(code), "Кастомная ошибка должна быть в диапозоне от 10000 до 29999.");

            return new ErrorCode(name, code);
        }

        public static implicit operator string(ErrorCode errorCode) => errorCode.Name;
        public static implicit operator int(ErrorCode errorCode) => errorCode.Code;

        public override string ToString() => Code.ToString();
    }
}
