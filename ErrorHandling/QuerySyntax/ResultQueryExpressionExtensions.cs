 using System;

namespace ErrorHandling.QuerySyntax;

public static class ResultQueryExpressionExtensions
{
    public static Result<TOutput> SelectMany<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.Then(continuation);
    }

    public static Result<TSelected> SelectMany<TInput, TOutput, TSelected>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation,
        Func<TInput, TOutput, TSelected> resultSelector)
    {
        var output = input.SelectMany(continuation);
        return output.IsSuccess ? input.Then(inp => resultSelector(inp, output.Value)) : Result.Fail<TSelected>(output.Error);
    }
}