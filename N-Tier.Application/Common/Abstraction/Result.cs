using System.Diagnostics.CodeAnalysis;

namespace N_Tier.Application.Common.Abstraction;

public class Result<T> : Result
{
    [MemberNotNullWhen(true, nameof(Data))]
    [MemberNotNullWhen(false, nameof(Failure))]
    public override bool IsSuccess => base.IsSuccess;

    [MemberNotNullWhen(true, nameof(Failure))]
    [MemberNotNullWhen(false, nameof(Data))]
    public bool IsFailed => !IsSuccess;

    public T? Data { get; private set; }
    public override Error? Failure => base.Failure;

    public Result(T data)
        : base()
    {
        Data = data;
    }

    public Result(Error error)
        : base(error) { }

    public static Result<TData> Success<TData>(TData data) => new(data);

    public static new Result<T> Fail(Error error) => new(error);

    public static implicit operator Result<T>(T data) => Success(data);

    public static implicit operator Result<T>(Error error) => Fail(error);
}

public class Result
{
    public Result(Error error)
    {
        IsSuccess = false;
        Failure = error;
    }

    public Result()
    {
        IsSuccess = true;
    }

    public virtual bool IsSuccess { get; private set; }
    public virtual Error? Failure { get; private set; }

    public static Result Success() => new();

    public static Result Fail(Error error) => new(error);

    public static implicit operator Result(Error error) => Fail(error);
}