using Skuttoo.Application.Dtos;
using Skuttoo.Domain.Enums;

namespace Skuttoo.Application.Services;

public interface ISubjectService
{
    Task<IReadOnlyList<SubjectDto>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>Subject + levels. Throws <see cref="Exceptions.NotFoundException"/> if unknown.</summary>
    Task<SubjectDetailDto> GetByKeyAsync(SubjectKey key, CancellationToken cancellationToken);
}
