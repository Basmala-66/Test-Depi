namespace E_commerce_System.results
{
    public class GenericResult<T> : Result
    {
        public T Value { get; }
        protected GenericResult(bool isSuccess,T value, string erorr) : base(isSuccess, erorr)
        {
            Value = value;
        }

        public static GenericResult<T> Success(T value)
            => new GenericResult<T>(true, value, string.Empty);
        public static GenericResult<T> Faliuar(string erorr)
            => new GenericResult<T>(false, default, erorr);


    }
}
