using Ardalis.Result;

namespace BuildingBlocks.Common.Helpers;

public static class ResultExtensions
{
    extension<T>(Result<T> source)
    {
        public bool IsFailure => !source.IsSuccess;
    }
}
