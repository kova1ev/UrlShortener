using UrlShortener.Application.Services;

namespace Application.UnitTests.Services;

public class AliasGeneratorTests
{
    [Fact]
    public void Generate_alias_Success()
    {
        //arrange
        AliasGenerator aliasGenerator = new();

        //act
        var alias = aliasGenerator.GenerateAlias();

        //assert
        Assert.NotEmpty(alias);
        Assert.NotNull(alias);
        Assert.InRange(alias.Length, 4, 10);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-2)]
    public void Generate_alias_by_minLength_Failure(int minLength)
    {
        //arrange
        AliasGenerator aliasGenerator = new();
        //act
        //assert
        Assert.Throws<ArgumentException>(() => aliasGenerator.GenerateAlias(minLength));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(1)]
    [InlineData(-2)]
    public void Generate_alias_by_maxLength_Failure(int maxLength)
    {
        //arrange
        AliasGenerator aliasGenerator = new();
        //act
        //assert
        Assert.Throws<ArgumentException>(() => aliasGenerator.GenerateAlias(maxLength: maxLength));
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(5, 2)]
    [InlineData(-1, 10)]
    public void Generate_alias_Failure(int minLength, int maxLength)
    {
        //arrange
        AliasGenerator aliasGenerator = new();

        //act
        //assert
        Assert.Throws<ArgumentException>(() => aliasGenerator.GenerateAlias(minLength, maxLength));
    }
}