namespace CostsSettler.Domain.Enums;
public enum CircumstanceStatus : byte
{
    None = 0,
    New = 1,
    PartiallyAccepted = 2,
    Accepted = 3,
    PartiallySettled = 4,
    Settled = 5,
    Rejected = 6,
}
