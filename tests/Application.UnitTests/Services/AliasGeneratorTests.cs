using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;

namespace Application.UnitTests.Services;

public class AliasGeneratorTests
{
    private readonly IAliasGenerator _aliasGenerator = new AliasGenerator();

    [Fact]
    public void GenerateAlias_WithDefaultParams_Should_return_validAlias()
    {
        //arrange
        int defaultMinLen = 4;
        int defaultMaxLen = 10;
        //act
        var alias = _aliasGenerator.GenerateAlias();

        //assert
        Assert.NotEmpty(alias);
        Assert.NotNull(alias);
        Assert.InRange(alias.Length, defaultMinLen, defaultMaxLen);
    }

    [Theory]
    [MemberData(nameof(ValidLengthForAlias))]
    public void GenerateAlias_InputValidParams_Should_return_ValidAlias(int minLen, int maxLen)
    {
        //act
        var alias = _aliasGenerator.GenerateAlias(minLen, maxLen);

        //assert
        Assert.NotEmpty(alias);
        Assert.NotNull(alias);
        Assert.InRange(alias.Length, minLen, maxLen);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    [InlineData(int.MinValue)]
    public void GenerateAlias_Should_throw_ArgumentException_WhenMinLenInvalid(int minLength)
    {
        //assert
        Assert.Throws<ArgumentException>(() => _aliasGenerator.GenerateAlias(minLength));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(1)]
    [InlineData(-2)]
    public void GenerateAlias_Should_throw_ArgumentException_WhenMaxLenInvalid(int maxLength)
    {
        //assert
        Assert.Throws<ArgumentException>(() => _aliasGenerator.GenerateAlias(maxLength: maxLength));
    }

    [Theory]
    [MemberData(nameof(InvalidLengthForAlias))]
    public void GenerateAlias_Should_throw_ArgumentException_WhenMinAndMax_LenInvalid(int minLength, int maxLength)
    {
        //assert
        Assert.Throws<ArgumentException>(() => _aliasGenerator.GenerateAlias(minLength, maxLength));
    }


    public static IEnumerable<object[]> InvalidLengthForAlias = new List<object[]>()
    {
        new object[] { 0, 2 },
        new object[] { int.MinValue, 2 },
        new object[] { -1, 111 }
    };

    public static IEnumerable<object[]> ValidLengthForAlias = new List<object[]>()
    {
        new object[] { 5, 7 },
        new object[] { 6, 12 },
        new object[] { 5, 5 }
    };
}

