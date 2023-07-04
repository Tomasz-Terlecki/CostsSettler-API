namespace CostsSettler.Domain.Enums;
public enum ChargeStatus : byte
{
    None = 0,
    New = 1,
    Accepted = 2,
    Rejected = 3,
    Settled = 4,
}
