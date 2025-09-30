// Em: FluxoSistema.Core/Models/LicenseFile.cs
using System;

namespace FluxoSistema.Core.Models
{
    public class LicenseFile
    {
        public List<string> Cnpjs { get; set; }
        public string Cnpj { get; set; }
        public string HardwareId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Signature { get; set; } // A assinatura digital completa
    }
}