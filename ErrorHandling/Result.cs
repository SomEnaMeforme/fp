using System;

namespace ErrorHandling;

public class None
{
    private None()
    {
    }
}

public struct Result<T>
{
    public Result(string error, T value = default)
    {
        Error = error;
        Value = value;
    }

    public string Error { get; }
    internal T Value { get; }

    public T GetValueOrThrow()
    {
        if (IsSuccess) return Value;
        throw new InvalidOperationException($"No value. Only Error {Error}");
    }

    public T GetValueOrDefault(T defaultValue = default)
    {
        return IsSuccess ? Value : defaultValue;
    }

    public bool IsSuccess => Error == null;
}

public static class Result
{
    public static Result<T> AsResult<T>(this T value)
    {
        return Ok(value);
    }
    public static Result<None> Ok()
    {
        return Ok<None>(null);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, value);
    }

    public static Result<T> Fail<T>(string e)
    {
        return new Result<T>(e);
    }

    public static Result<T> Of<T>(Func<T> f, string error = null)
    {
        try
        {
            return Ok(f());
        }
        catch (Exception e)
        {
            return Fail<T>(error ?? e.Message);
        }
    }

    public static Result<None> OfAction(Action f, string error = null)
    {
        try
        {
            f();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail<None>(error ?? e.Message);
        }
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        return input.Then(i => Of(() => continuation(i)));
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess ? continuation(input.Value) : Fail<TOutput>(input.Error);
    }
    public static Result<None> Then<TInput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    public static Result<None> Then<TInput>(
        this Result<TInput> input,
        Action<Result<TInput>> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    public static Result<TInput> OnFail<TInput>(
        this Result<TInput> input,
        Action<string> handleError)
    {
        if (input.IsSuccess) return input;
        handleError(input.Error);
        return Fail<TInput>(input.Error);
    }

    public static Result<TInput> Validate<TInput>(
        this Result<TInput> input, 
        Func<TInput, bool> predicate, 
        string errorMessage)
    {
        return input.IsSuccess && predicate(input.Value)
            ? Ok(input.Value)
            : Fail<TInput>(errorMessage);
    }

    public static Result<TInput> ValidateOrGetDefault<TInput>(
        this Result<TInput> input,
        Func<TInput, bool> predicate, 
        TInput defaultValue)
    {
        return input.IsSuccess && predicate(input.Value)
            ? Ok(input.Value)
            : Ok(defaultValue);
    }

    public static Result<TInput> ReplaceError<TInput>(
        this Result<TInput> input,
        Func<Result<TInput>, string> getNewError)
    {
        return input.IsSuccess ? input : Fail<TInput>(getNewError(input));
    }

    public static Result<TInput> RefineError<TInput>(
        this Result<TInput> input,
        string errorMessage)
    {
        return input.ReplaceError(i => $"{errorMessage}. {i.Error}");
    }
}