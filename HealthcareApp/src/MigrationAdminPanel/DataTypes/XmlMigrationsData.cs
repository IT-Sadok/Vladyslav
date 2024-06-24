using System.Xml.Serialization;
using Domain.Entities;

namespace MigrationAdminPanel.DataTypes;

[Serializable]
[XmlRoot("data")]
public class XmlMigrationsData
{
    [XmlArray("doctors")]
    [XmlArrayItem("doctor")]
    public List<ApplicationUser> Doctors { get; set; }

    [XmlArray("patients")]
    [XmlArrayItem("patient")]
    public List<ApplicationUser> Patients { get; set; }
}
