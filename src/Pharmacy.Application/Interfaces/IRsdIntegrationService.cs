using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Interfaces;

public interface IRsdIntegrationService
{
    Task<AcceptDispatchResponseDto> AcceptDispatchAsync(AcceptDispatchRequestDto request, CancellationToken cancellationToken = default);
    Task<DispatchDetailResponseDto> GetDispatchDetailAsync(DispatchDetailRequestDto request, CancellationToken cancellationToken = default);
    Task<AcceptBatchResponseDto> AcceptBatchAsync(AcceptBatchRequestDto request, CancellationToken cancellationToken = default);
    Task<PharmacySaleResponseDto> PharmacySaleAsync(PharmacySaleRequestDto request, CancellationToken cancellationToken = default);
    Task<PharmacySaleCancelResponseDto> PharmacySaleCancelAsync(PharmacySaleCancelRequestDto request, CancellationToken cancellationToken = default);
    Task<StakeholderListResponseDto> GetStakeholderListAsync(StakeholderListRequestDto request, CancellationToken cancellationToken = default);
    Task<ReturnBatchResponseDto> ReturnBatchAsync(ReturnBatchRequestDto request, CancellationToken cancellationToken = default);
}