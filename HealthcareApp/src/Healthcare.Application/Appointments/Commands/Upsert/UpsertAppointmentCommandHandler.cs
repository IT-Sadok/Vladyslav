using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Appointments.Commands.Upsert;

public class UpsertAppointmentCommandHandler : IRequestHandler<UpsertAppointmentCommand, Result<string>>
{
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;


    public UpsertAppointmentCommandHandler(IAppointmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpsertAppointmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var appointment = _mapper.Map<Appointment>(request);
            await _repository.UpsertAppointmentAsync(appointment);
            return Result<string>.Success();
        }
        catch (Exception e)
        {
            return Result<string>.Failure(e.Message);
        }
    }
}