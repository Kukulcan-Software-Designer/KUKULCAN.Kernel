using System.ComponentModel.DataAnnotations;
using ATLAS.SharedKernel.Primitives.Interfaces;

namespace ATLAS.SharedKernel.Primitives;

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