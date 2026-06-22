using Skuttoo.Application.Abstractions;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Exceptions;
using Skuttoo.Application.Mapping;
using Skuttoo.Domain.Enums;

namespace Skuttoo.Application.Services;

public sealed class SubjectService(ISubjectRepository subjects) : ISubjectService
{
    private readonly ISubjectRepository _subjects = subjects;

    public async Task<IReadOnlyList<SubjectDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _subjects.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return ContentMapper.ToDtoList(entities);
    }

    public async Task<SubjectDetailDto> GetByKeyAsync(SubjectKey key, CancellationToken cancellationToken)
    {
        var subject = await _subjects.GetByKeyWithLevelsAsync(key, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException("Subject", key);

        return ContentMapper.ToDetailDto(subject);
    }
}
