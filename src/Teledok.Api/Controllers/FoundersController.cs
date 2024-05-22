using Ardalis.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teledok.Application.Dtos;
using Teledok.Application.Specifications;
using Teledok.Core.Exceptions;
using Teledok.Core.Helpers;
using Teledok.Domain.Entities;
using Teledok.Infrastructure.Abstractions.Repositories;
using Teledok.Infrastructure.EntityFrameworkCore.Specifications;

namespace Teledok.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoundersController : ControllerBase
{
    private readonly ILogger<FoundersController> _logger;
    private readonly IReadEntityRepository<int, Founder> _readFoundersEntityRepository;
    private readonly IReadEntityRepository<int, Client> _readClientsEntityRepository;
    private readonly IEntityRepository<int, Founder> _entityRepository;
    private readonly IMapper _mapper;

    public FoundersController(ILogger<FoundersController> logger, IReadEntityRepository<int, Founder> readFoundersEntityRepository,
        IEntityRepository<int, Founder> entityRepository, IMapper mapper, IReadEntityRepository<int, Client> readClientsEntityRepository)
    {
        ArgumentNullException.ThrowIfNull(readFoundersEntityRepository, nameof(readFoundersEntityRepository));
        ArgumentNullException.ThrowIfNull(readClientsEntityRepository, nameof(readClientsEntityRepository));
        ArgumentNullException.ThrowIfNull(entityRepository, nameof(entityRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        
        _readClientsEntityRepository = readClientsEntityRepository;
        _readFoundersEntityRepository = readFoundersEntityRepository;
        _entityRepository = entityRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<DataResponse<IReadOnlyCollection<FounderDto>>> GetAll(int limit, CancellationToken cancellationToken)
    {
        if (limit is < 1)
            throw new BadRequestException(ErrorMessages.LimitOutOfRange, nameof(limit));
        
        var specifications = new List<ISpecification<Founder>>()
        {
            new FounderIncludeSpecification(),
            new LimitSpecification<Founder>(limit)
        };

        var entities = await _readFoundersEntityRepository.GetAllAsync(specifications, true, cancellationToken);
        var mappedEntities = _mapper.Map<IReadOnlyCollection<FounderDto>>(entities);
        
        return DataResponse<IReadOnlyCollection<FounderDto>>.Success(mappedEntities);
    }

    [HttpGet("{id}")]
    public async Task<DataResponse<FounderDto>> GetFounder(int id, CancellationToken cancellationToken = default)
    {
        if (id is < 1)
            throw new BadRequestException(ErrorMessages.IdOutOfRange, nameof(id));
        
        var founderIncludeSpecification = new FounderIncludeSpecification();
        var founder = await _readFoundersEntityRepository.GetAsync(founderIncludeSpecification, true, cancellationToken);

        if (founder is null)
            throw new NotFoundException(nameof(Founder), id);

        var mappedEntity = _mapper.Map<FounderDto>(founder);

        return DataResponse<FounderDto>.Success(mappedEntity);
    }

    [HttpPost]
    public async Task<BaseResponse> CreateFounder(FounderDto founderDto, CancellationToken cancellationToken)
    {
        if (founderDto is null)
            throw new BadRequestException(ErrorMessages.CantBeNull, nameof(founderDto));

        var isClientIdExists = await _readClientsEntityRepository.GetByIdAsync(
            founderDto.ClientId, true, cancellationToken);
        
        if (isClientIdExists is null)
            throw new NotFoundException(nameof(Client), founderDto.ClientId);
        
        var isExists = await _readFoundersEntityRepository.GetAsync(
            new FounderUniqueSpecification(founderDto.INN),
            true, cancellationToken);

        if (isExists is not null)
            throw new BadRequestException(ErrorMessages.EntityAlreadyExists, $"{founderDto.INN}"); 
        
        var mappedEntity = _mapper.Map<Founder>(founderDto);
        var founder = await _entityRepository.AddAsync(mappedEntity, true, cancellationToken);
        
        _logger.LogInformation(SuccessMessages.EntityCreated<int, Founder>(founder.Id));

        return BaseResponse.Success();
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateFounder(int id, FounderDto founderDto, CancellationToken cancellationToken)
    {
        if (id is < 1)
            throw new BadRequestException(ErrorMessages.IdOutOfRange, nameof(id));
        
        if (founderDto is null)
            throw new BadRequestException(ErrorMessages.CantBeNull, nameof(founderDto));

        var isClientIdExists = await _readClientsEntityRepository.GetByIdAsync(
            founderDto.ClientId, true, cancellationToken);
        
        if (isClientIdExists is null)
            throw new NotFoundException(nameof(Client), founderDto.ClientId);
        
        var isExists = await _readFoundersEntityRepository.GetAsync(
            new FounderUniqueSpecification(founderDto.INN),
            true, cancellationToken);

        if (isExists is not null)
            throw new BadRequestException(ErrorMessages.EntityAlreadyExists, $"{founderDto.INN}"); 

        var entityToUpdate = await _readFoundersEntityRepository.GetByIdAsync(id, true, cancellationToken);
        
        if (entityToUpdate is null)
            throw new NotFoundException(nameof(Founder), id);
        
        _mapper.Map(founderDto, entityToUpdate);
        await _entityRepository.UpdateAsync(entityToUpdate, true, cancellationToken);
        
        _logger.LogInformation(SuccessMessages.EntityUpdated<int, Founder>(id));

        return BaseResponse.Success();
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse> DeleteFounder(int id, CancellationToken cancellationToken)
    {
        var founder = await _readFoundersEntityRepository.GetByIdAsync(id, true, cancellationToken);
        
        if (founder is null)
            throw new NotFoundException(nameof(Founder), id);

        await _entityRepository.DeleteAsync(founder, true, cancellationToken);
        
        _logger.LogInformation(SuccessMessages.EntityDeleted<int, Founder>(founder.Id));
        
        return BaseResponse.Success();
    }
}