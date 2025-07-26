namespace LRA.Shared.Abstractions.Dtos;

public record GenericServiceResponse<T>(T Response)
{
}
