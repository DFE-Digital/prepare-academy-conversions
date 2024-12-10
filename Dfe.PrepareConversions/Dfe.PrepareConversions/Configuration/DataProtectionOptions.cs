namespace Dfe.PrepareConversions.Configuration;

public class DataProtectionOptions
{
   public const string ConfigurationSection = "DataProtection";
   public string KeyVaultKey { get; init; }
}
