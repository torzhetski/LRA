namespace LRA.Shared.Abstractions.Test;

public static class TestMapper
{
    public static TestModel FromDtoToModel(this TestDto dto)
    {
        return new TestModel()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
        };
    }

    public static TestDto FromModelToDto(this TestModel model)
    {
        return new TestDto()
        {
            Name = model.Name,
        };
    }
}
