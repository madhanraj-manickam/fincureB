using FinCure.DTOs.InvestmentDTO;
using FinCure.Models;
using FinCure.Repositories.Interfaces;

namespace FinCure.Services
{
    public class InvestmentService
    {
        private readonly IInvestmentRepository _investmentRepository;

        public InvestmentService(
            IInvestmentRepository investmentRepository)
        {
            _investmentRepository = investmentRepository;
        }

        public async Task<InvestmentResponseDto> AddInvestmentAsync(
            int userId,
            CreateInvestmentDto dto)
        {
            var investment = new Investment
            {
                UserId = userId,
                InvestmentName = dto.InvestmentName,
                InvestmentType = dto.InvestmentType,
                AmountInvested = dto.AmountInvested,
                StartDate = dto.StartDate,
              
                Notes = dto.Notes
            };

            await _investmentRepository.AddAsync(investment);

            return MapToResponseDto(investment);
        }

        public async Task<IEnumerable<InvestmentResponseDto>> GetAllAsync(
            int userId)
        {
            var investments =
                await _investmentRepository.GetAllByUserIdAsync(userId);

            return investments.Select(MapToResponseDto);
        }

        public async Task<InvestmentResponseDto?> GetByIdAsync(
            int id,
            int userId)
        {
            var investment =
                await _investmentRepository.GetByIdAsync(id);

            if (investment == null || investment.UserId != userId)
                return null;

            return MapToResponseDto(investment);
        }

        public async Task<InvestmentResponseDto?> UpdateAsync(
            int id,
            int userId,
            UpdateInvestmentDto dto)
        {
            var investment =
                await _investmentRepository.GetByIdAsync(id);

            if (investment == null || investment.UserId != userId)
                return null;

            investment.InvestmentName = dto.InvestmentName;
            investment.InvestmentType = dto.InvestmentType;
            investment.AmountInvested = dto.AmountInvested;
            investment.StartDate = dto.StartDate;
            
            investment.Notes = dto.Notes;

            await _investmentRepository.UpdateAsync(investment);

            return MapToResponseDto(investment);
        }

        public async Task<bool> DeleteAsync(
            int id,
            int userId)
        {
            var investment =
                await _investmentRepository.GetByIdAsync(id);

            if (investment == null || investment.UserId != userId)
                return false;

            await _investmentRepository.DeleteAsync(id);

            return true;
        }

        private static InvestmentResponseDto MapToResponseDto(
            Investment investment)
        {
            return new InvestmentResponseDto
            {
                InvestmentId = investment.InvestmentId,
                InvestmentName = investment.InvestmentName,
                InvestmentType = investment.InvestmentType,
                AmountInvested = investment.AmountInvested,
                StartDate = investment.StartDate,
                Notes = investment.Notes
            };
        }
    }
}