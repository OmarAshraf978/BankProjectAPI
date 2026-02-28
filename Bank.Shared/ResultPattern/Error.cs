using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared.ResultPattern
{
    #region ResultPattern
    public class Error
    {
        public string Code { get; set; }
        public String Description { get; set; }
        public ErrorTypes Type { get; set; }
        public Error(string code, string description, ErrorTypes type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public static Error Failure(string code = "General.Failure", string description = "General Failure Has Occurred")
        {
            return new Error(code, description, ErrorTypes.Failure);
        }
        public static Error Validation(string code = "General.Validation", string description = "Validation Error Has Occurred")
        {
            return new Error(code, description, ErrorTypes.Validation);
        }
        public static Error NotFound(string code = "General.NotFound", string description = "The Requested Resource Was Not Found")
        {
            return new Error(code, description, ErrorTypes.NotFound);
        }
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "You Are Not Authorized")
        {
            return new Error(code, description, ErrorTypes.Unauthorized);
        }
        public static Error Forbidden(string code = "General.Forbidden", string description = "You Are Not Have Permission")
        {
            return new Error(code, description, ErrorTypes.Forbidden);
        }
        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "Invalid username or password")
        {
            return new Error(code, description, ErrorTypes.InvalidCredentials);
        }
    }
    public enum ErrorTypes
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Unauthorized = 3,
        Forbidden = 4,
        InvalidCredentials = 5
    }
    public class Result
    {
        private readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0;
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors => _errors;
        protected Result() { }
        protected Result(Error error) { _errors.Add(error); }
        protected Result(List<Error> errors) { _errors.AddRange(errors); }
        public static Result Ok() => new Result();
        public static Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);
    }
    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");
        private Result(TValue value) : base() { _value = value; }
        private Result(Error error) : base(error) { _value = default!; }
        private Result(List<Error> errors) : base(errors) { _value = default!; }
        public static Result<TValue> Ok(TValue value) => new(value);
        public static new Result<TValue> Fail(Error error) => new(error);
        public static new Result<TValue> Fail(List<Error> errors) => new(errors);

        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
    #endregion
}
