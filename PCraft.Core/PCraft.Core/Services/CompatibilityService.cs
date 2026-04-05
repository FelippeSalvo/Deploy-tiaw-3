using PCraft.Core.DTOs;
using PCraft.Core.Models;

namespace PCraft.Core.Services
{
    public class CompatibilityService : ICompatibilityService
    {
        private readonly List<IValidadorCompatibilidade> _validadores;

        public CompatibilityService()
        {
            _validadores = new List<IValidadorCompatibilidade>
            {
                new ValidadorProcessadorPlacaMae(),
                new ValidadorRamPlacaMae(),
                new ValidadorGpuFonte(),
                new ValidadorFonteSistema()
            };
        }

        public RespostaVerificacaoCompatibilidade VerificarCompatibilidade(
            CPU cpu,
            Motherboard motherboard,
            RAM ram,
            GPU gpu,
            PSU psu)
        {
            var problemas = new List<ProblemaCompatibilidade>();
            var contexto = new ContextoCompatibilidade { CPU = cpu, Motherboard = motherboard, RAM = ram, GPU = gpu, PSU = psu };

            foreach (var validador in _validadores)
                validador.Validar(contexto, problemas);

            var consumoTotal = CalcularConsumoTotal(cpu, gpu, ram);

            return new RespostaVerificacaoCompatibilidade
            {
                Compativel = !problemas.Any(p => p.Severidade == "Critico"),
                Problemas = problemas,
                ConsumoTotalEnergia = consumoTotal,
                PSURecomendada = (int)(consumoTotal * 1.2)
            };
        }

        private int CalcularConsumoTotal(CPU cpu, GPU gpu, RAM ram)
        {
            var total = 0;
            if (cpu != null) total += cpu.TDP;
            if (gpu != null) total += gpu.TDP;
            total += 50; // Placa-mãe
            if (ram != null) total += ram.QuantidadeModulos * 3;
            total += 30; // Outros
            return total;
        }
    }

    public class ContextoCompatibilidade
    {
        public CPU CPU { get; set; }
        public Motherboard Motherboard { get; set; }
        public RAM RAM { get; set; }
        public GPU GPU { get; set; }
        public PSU PSU { get; set; }
    }

    public interface IValidadorCompatibilidade
    {
        void Validar(ContextoCompatibilidade contexto, List<ProblemaCompatibilidade> problemas);
    }

    public class ValidadorProcessadorPlacaMae : IValidadorCompatibilidade
    {
        public void Validar(ContextoCompatibilidade ctx, List<ProblemaCompatibilidade> problemas)
        {
            if (ctx.CPU == null || ctx.Motherboard == null) return;

            if (ctx.CPU.Socket != ctx.Motherboard.Socket)
            {
                problemas.Add(new ProblemaCompatibilidade
                {
                    Componente1 = "CPU",
                    Componente2 = "Motherboard",
                    Problema = $"Socket incompatível: CPU ({ctx.CPU.Socket}) vs Motherboard ({ctx.Motherboard.Socket})",
                    Severidade = "Critico"
                });
            }
        }
    }

    public class ValidadorRamPlacaMae : IValidadorCompatibilidade
    {
        public void Validar(ContextoCompatibilidade ctx, List<ProblemaCompatibilidade> problemas)
        {
            if (ctx.RAM == null || ctx.Motherboard == null) return;

            if (ctx.RAM.Tipo != ctx.Motherboard.TipoRamSuportado)
            {
                problemas.Add(new ProblemaCompatibilidade
                {
                    Componente1 = "RAM",
                    Componente2 = "Motherboard",
                    Problema = $"Tipo de RAM incompatível: {ctx.RAM.Tipo} vs {ctx.Motherboard.TipoRamSuportado}",
                    Severidade = "Critico"
                });
            }

            var capacidadeTotal = ctx.RAM.Capacidade * ctx.RAM.QuantidadeModulos;
            if (capacidadeTotal > ctx.Motherboard.CapacidadeMaximaRam)
            {
                problemas.Add(new ProblemaCompatibilidade
                {
                    Componente1 = "RAM",
                    Componente2 = "Motherboard",
                    Problema = $"Capacidade RAM ({capacidadeTotal}GB) excede máximo ({ctx.Motherboard.CapacidadeMaximaRam}GB)",
                    Severidade = "Critico"
                });
            }

            if (ctx.RAM.QuantidadeModulos > ctx.Motherboard.SlotsRam)
            {
                problemas.Add(new ProblemaCompatibilidade
                {
                    Componente1 = "RAM",
                    Componente2 = "Motherboard",
                    Problema = $"Módulos RAM ({ctx.RAM.QuantidadeModulos}) excede slots ({ctx.Motherboard.SlotsRam})",
                    Severidade = "Critico"
                });
            }
        }
    }

    public class ValidadorGpuFonte : IValidadorCompatibilidade
    {
        public void Validar(ContextoCompatibilidade ctx, List<ProblemaCompatibilidade> problemas)
        {
            if (ctx.GPU == null || ctx.PSU == null) return;

            if (ctx.PSU.Potencia < ctx.GPU.ConsumoRecomendado)
            {
                problemas.Add(new ProblemaCompatibilidade
                {
                    Componente1 = "GPU",
                    Componente2 = "PSU",
                    Problema = $"PSU ({ctx.PSU.Potencia}W) insuficiente para GPU (mín. {ctx.GPU.ConsumoRecomendado}W)",
                    Severidade = "Critico"
                });
            }
        }
    }

    public class ValidadorFonteSistema : IValidadorCompatibilidade
    {
        public void Validar(ContextoCompatibilidade ctx, List<ProblemaCompatibilidade> problemas)
        {
            if (ctx.PSU == null) return;

            var total = 0;
            if (ctx.CPU != null) total += ctx.CPU.TDP;
            if (ctx.GPU != null) total += ctx.GPU.TDP;
            total += 50;
            if (ctx.RAM != null) total += ctx.RAM.QuantidadeModulos * 3;
            total += 30;

            if (ctx.PSU.Potencia < total)
            {
                problemas.Add(new ProblemaCompatibilidade
                {
                    Componente1 = "PSU",
                    Componente2 = "Sistema",
                    Problema = $"PSU ({ctx.PSU.Potencia}W) insuficiente para consumo total ({total}W)",
                    Severidade = "Critico"
                });
            }
        }
    }
}
