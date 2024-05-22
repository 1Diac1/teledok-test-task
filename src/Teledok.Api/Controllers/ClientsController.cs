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
public class ClientsController : ControllerBase
{
    private readonly ILogger<ClientsController> _logger;
    private readonly IReadEntityRepository<int, Client> _readEntityRepository;
    private readonly IEntityRepository<int, Client> _entityRepository;
    private readonly IMapper _mapper;

    public ClientsController(ILogger<ClientsController> logger, IReadEntityRepository<int, Client> readEntityRepository,
        IEntityRepository<int, Client> entityRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(readEntityRepository, nameof(readEntityRepository));
        ArgumentNullException.ThrowIfNull(entityRepository, nameof(entityRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        
        _readEntityRepository = readEntityRepository;
        _entityRepository = entityRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<DataResponse<IReadOnlyCollection<ClientDto>>> GetAll(int limit, CancellationToken cancellationToken)
    {
        if (limit is < 1)
            throw new BadRequestException(ErrorMessages.LimitOutOfRange, nameof(limit));
        
        var specifications = new List<ISpecification<Client>>()
        {
            new ClientsIncludeSpecification(),
            new LimitSpecification<Client>(limit)
        };

        var entities = await _readEntityRepository.GetAllAsync(specifications, true, cancellationToken);
        var mappedEntities = _mapper.Map<IReadOnlyCollection<ClientDto>>(entities);
        
        return DataResponse<IReadOnlyCollection<ClientDto>>.Success(mappedEntities);
    }

    [HttpGet("{id}")]
    public async Task<DataResponse<ClientDto>> GetClient(int id, CancellationToken cancellationToken = default)
    {
        if (id is < 1)
            throw new BadRequestException(ErrorMessages.IdOutOfRange, nameof(id));
        
        var clientsIncludeSpecification = new ClientsIncludeSpecification();
        var client = await _readEntityRepository.GetAsync(clientsIncludeSpecification, true, cancellationToken);

        if (client is null)
            throw new NotFoundException(nameof(Client), id);

        var mappedEntity = _mapper.Map<ClientDto>(client);

        return DataResponse<ClientDto>.Success(mappedEntity);
    }

    [HttpPost]
    public async Task<BaseResponse> CreateClient(ClientDto clientDto, CancellationToken cancellationToken)
    {
        if (clientDto is null)
            throw new BadRequestException(ErrorMessages.CantBeNull, nameof(clientDto));

        var isExists = await _readEntityRepository.GetAsync(
            e => e.INN == clientDto.INN ||
                 e.Name == clientDto.Name,
            true,
            cancellationToken);
        
        if (isExists is not null)
            throw new BadRequestException(ErrorMessages.EntityAlreadyExists, $"{clientDto.Name}/{clientDto.INN}"); 
        
        var mappedEntity = _mapper.Map<Client>(clientDto);
        var client = await _entityRepository.AddAsync(mappedEntity, true, cancellationToken);
        
        _logger.LogInformation(SuccessMessages.EntityCreated<int, Client>(client.Id));

        return BaseResponse.Success();
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateClient(int id, ClientDto clientDto, CancellationToken cancellationToken)
    {
        if (id is < 1)
            throw new BadRequestException(ErrorMessages.IdOutOfRange, nameof(id));
        
        if (clientDto is null)
            throw new BadRequestException(ErrorMessages.CantBeNull, nameof(clientDto));

        var isExists = await _readEntityRepository.GetAsync(
            e => e.INN == clientDto.INN ||
            e.Name == clientDto.Name,
            true,
            cancellationToken);
        
        if (isExists is not null)
            throw new BadRequestException(ErrorMessages.EntityAlreadyExists, $"{clientDto.Name}/{clientDto.INN}");  

        var entityToUpdate = await _readEntityRepository.GetByIdAsync(id, true, cancellationToken);
        
        if (entityToUpdate is null)
            throw new NotFoundException(nameof(Client), id);
        
        _mapper.Map(clientDto, entityToUpdate);
        await _entityRepository.UpdateAsync(entityToUpdate, true, cancellationToken);
        
        _logger.LogInformation(SuccessMessages.EntityUpdated<int, Client>(id));

        return BaseResponse.Success();
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse> DeleteClient(int id, CancellationToken cancellationToken)
    {
        var client = await _readEntityRepository.GetByIdAsync(id, true, cancellationToken);
        
        if (client is null)
            throw new BadRequestException(ErrorMessages.CantBeNull, nameof(client));

        await _entityRepository.DeleteAsync(client, true, cancellationToken);
        
        _logger.LogInformation(SuccessMessages.EntityDeleted<int, Client>(client.Id));
        
        return BaseResponse.Success();
    }
}