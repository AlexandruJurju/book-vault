using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Domain;

public interface IHasConcurrency
{
    [Timestamp]
    uint Version { get; set; }
}
