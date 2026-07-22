using Ardalis.Result;

namespace BuildingBlocks.Common.Extensions;

public static class ResultExtensions
{
    extension<T>(Result<T> source)
    {
        public bool IsFailure => !source.IsSuccess;
    }
}
