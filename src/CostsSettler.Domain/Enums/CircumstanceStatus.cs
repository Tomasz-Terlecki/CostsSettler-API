namespace CostsSettler.Domain.Enums;
public enum CircumstanceStatus : byte
{
    None = 0,
    New = 1,
    PartiallyAccepted = 2,
    Accepted = 3,
    Settled = 4,
}
