using System.ComponentModel.DataAnnotations;
using ATLAS.Kernel.Primitives.Interfaces;

namespace ATLAS.Kernel.Primitives;

/// <summary>
/// Practical base class for entities that support logical deletion (soft delete).
/// </summary>
public abstract class ErasableEntity : BaseEntity, ISoftDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether this entity has been marked as deleted.
    /// </summary>
    [Required]
    public bool Erased { get; set; }
}