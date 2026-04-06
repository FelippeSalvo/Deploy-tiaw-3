using PCraft.Core.DTOs;
using PCraft.Core.Models;

namespace PCraft.Core.Services
{
    public interface ICompatibilityService
    {
        RespostaVerificacaoCompatibilidade VerificarCompatibilidade(
            CPU cpu,
            Motherboard motherboard,
            RAM ram,
            GPU gpu,
            PSU psu);
    }
}
