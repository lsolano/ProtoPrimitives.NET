namespace Triplex.ProtoDomainPrimitives.Temporal;

internal static class Constants
{
    internal const string ToISOStringFormat = "yyyy'-'MM'-'ddTHH':'mm':'ss'.'fff'Z'";
    internal static readonly DateTimeFormatInfo ToISOStringFormatInfo = DateTimeFormatInfo.InvariantInfo;
}
