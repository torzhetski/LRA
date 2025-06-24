namespace LRA.Shared.Abstractions.Test;

public static class TestMapper
{
    public static TestModel FromDtoToModel(this TestDto model)
    {
        return new TestModel()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
        };
    }
}
