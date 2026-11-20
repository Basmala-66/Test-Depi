namespace E_commerce_System.results
{
    public class Result
    {
        public bool IsSuccesse { get; }
        public bool IsFaliure => !IsSuccesse;

        public string Erorr { get; }

        protected Result(bool isSuccess,string erorr)
        {
            IsSuccesse = isSuccess;
            Erorr = erorr;
        }

        public static Result Succcess() => new Result(true, string.Empty);

        public static Result Faliure(string erorr) => new Result(false,erorr );
    }
}
