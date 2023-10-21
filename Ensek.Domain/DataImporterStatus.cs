namespace Ensek.Domain;

public enum DataImporterStatus
{
    Unknown = 0,
    New = 1,

    Loaded = 2,
    Loading = 3,

    Validating = 4,
    Validated = 5,
    
    Importing = 6,
    Imported = 7
}
