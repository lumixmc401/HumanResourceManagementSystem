using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace HumanResourceManagementSystem.Service.Implementations
{
    public class VoucherNumberGenerator : IVoucherNumberGenerator
    {
        private readonly IHumanResourceUnitOfWork _unitOfWork;
        private readonly ILogger<VoucherNumberGenerator> _logger;

        public VoucherNumberGenerator(
            IHumanResourceUnitOfWork unitOfWork,
            ILogger<VoucherNumberGenerator> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<string> GenerateVoucherNumberAsync(DateTime date)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                string dateString = date.ToString("yyyyMMdd");
                var sequence = await GetOrCreateSequence(dateString);
                string number = $"{dateString}{sequence.CurrentSequence:D2}";

                var voucherNumber = new VoucherNumber
                {
                    Id = Guid.NewGuid(),
                    Number = number,
                    CreateAt = DateTime.UtcNow
                };

                await _unitOfWork.VoucherNumbers.AddAsync(voucherNumber);
                await _unitOfWork.CommitTransactionAsync();

                return voucherNumber.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating voucher number");
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task<VoucherNumberSequence> GetOrCreateSequence(string dateString)
        {
            var sequence = await _unitOfWork.VoucherNumberSequences
                .FindFirstAsync(s => s.Date == dateString);

            if (sequence == null)
            {
                sequence = new VoucherNumberSequence
                {
                    Date = dateString,
                    CurrentSequence = 1
                };
                await _unitOfWork.VoucherNumberSequences.AddAsync(sequence);
            }
            else
            {
                sequence.CurrentSequence++;
                if (sequence.CurrentSequence > 99)
                {
                    throw new InvalidOperationException("Sequence overflow for the day");
                }
                await _unitOfWork.VoucherNumberSequences.UpdateAsync(sequence);
            }

            return sequence;
        }
    }
}
