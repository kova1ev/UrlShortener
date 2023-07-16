using System.ComponentModel.DataAnnotations;

namespace UrlShortener.ClientWasm.Models;

public class CreateLinkModel
{
    private string? _urlAddress;
    private string? _alias;

    [Required]
    [Url]
    public string? UrlAddress
    {
        get { return _urlAddress; }
        set { _urlAddress = value?.Trim(); }
    }

    [StringLength(30, MinimumLength = 3)]
    public string? Alias
    {
        get => _alias;
        set
        {
            var temp = value?.Trim();
            _alias = temp?.Length == 0 ? null : temp;
        }
    }

}