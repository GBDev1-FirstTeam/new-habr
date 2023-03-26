using System;
using System.Net.NetworkInformation;

namespace NewHabr.Domain;

public class Result
{
    public bool Success { get; private set; }
    public string Error { get; private set; }

    public bool Failure => !Success;

    protected Result(bool success, string error)
    {
        Success = success;
        Error = error;
    }


    public static Result Ok()
    {
        return new Result(true, string.Empty);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

    public static Result Fail(string message)
    {
        return new Result(false, message);
    }

    public static Result<T> Fail<T>(string message)
    {
        return new Result<T>(default(T)!, false, message);
    }
}


public class Result<T> : Result
{
    private T? _value;


    protected internal Result(T value, bool success, string message)
        : base(success, message)
    {
        Value = value;
    }

    public T? Value
    {
        get
        {
            if (Failure)
                throw new InvalidOperationException();

            return _value;
        }

        protected set
        {
            _value = value;
        }
    }
}