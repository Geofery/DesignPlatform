using System;
namespace UserManagementService.Web.Messaging
{
    using NServiceBus;
    using Shared.Contracts;

    public class UserCreatedEventHandler : IHandleMessages<UserCreatedEvent>
    {
        public Task Handle(UserCreatedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"UserCreatedEvent received for UserId: {message.UserId}");
            return Task.CompletedTask;
        }
    }
}


//TODO: IDA EKSEMPEL
/*
 using Ida.MemberSignupApi.Domain.Messages.Commands;
using Ida.MemberSignupApi.Domain.Messages.Messages;
using Ida.MemberSignupApi.Domain.Models;
using Ida.MemberSignupApi.Domain.Repositories;
using NServiceBus;

namespace Ida.MemberSignupApi.Web.Messaging.Student;

public class AddStudentMembershipFeeHandler : IHandleMessages<AddStudentMembershipFee>
{
    private readonly ILeadRepository _leadRepository;
    private readonly IMembershipFeeRepository _membershipFeeRepository;

    public AddStudentMembershipFeeHandler(IMembershipFeeRepository membershipFeeRepository,
        ILeadRepository leadRepository)
    {
        _membershipFeeRepository = membershipFeeRepository;
        _leadRepository = leadRepository;
    }

    public async Task Handle(AddStudentMembershipFee message, IMessageHandlerContext context)
    {
        if (message?.IdaUserNumber is null || message.LeadId is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var lead = await _leadRepository
            .GetAll()
            .ContinueWith(a =>
                a.Result.FirstOrDefault(l =>
                    l.Config.LeadId == message.LeadId))
            .ConfigureAwait(false) ?? throw new Exception("Lead not found");

        var existingMembershipFees = await _membershipFeeRepository
            .GetMembershipFees(message.IdaUserNumber)
            .ConfigureAwait(false);

        if (!existingMembershipFees.Any())
        {
            var membershipFee = new MembershipFee
            {
                MembershipFeeGroupName = "student",
                StartDate = DateTime.Now,
                IdaUserNumber = message.IdaUserNumber,
                PaymentFrequency = PaymentFrequency.QuarterlyCharging
            };
            await _membershipFeeRepository.Create(message.IdaUserNumber, membershipFee).ConfigureAwait(false);

            await context
                .Reply(new MembershipFeeAdded { LeadId = message.LeadId, Status = "" })
                .ConfigureAwait(false);
        }
    }
}
 */