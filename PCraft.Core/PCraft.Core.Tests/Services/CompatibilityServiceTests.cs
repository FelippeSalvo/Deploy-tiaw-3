using PCraft.Core.Models;
using PCraft.Core.Services;
using Xunit;

namespace PCraft.Core.Tests.Services;

public class CompatibilityServiceTests
{
    private readonly CompatibilityService _service;

    public CompatibilityServiceTests()
    {
        _service = new CompatibilityService();
    }

    // CPU + Motherboard

    [Fact]
    public void VerificarCompatibilidade_CPUeMotherboard_Compativeis_MesmoSocket()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.True(resultado.Compativel);
        Assert.Empty(resultado.Problemas.Where(p => p.Severidade == "Critico"));
    }

    [Fact]
    public void VerificarCompatibilidade_CPUeMotherboard_Incompativel_SocketDiferente()
    {
        var cpu = new CPU { Nome = "Ryzen 7 5800X", Fabricante = "AMD", Socket = CpuSocket.AM4, TDP = 105 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.False(resultado.Compativel);
        Assert.Contains(resultado.Problemas, p => p.Componente1 == "CPU" && p.Componente2 == "Motherboard" && p.Severidade == "Critico");
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("Socket incompatível"));
    }

    // RAM + Motherboard

    [Fact]
    public void VerificarCompatibilidade_RAMeMotherboard_Compativeis_MesmoTipoRam()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.True(resultado.Compativel);
        Assert.Empty(resultado.Problemas.Where(p => p.Componente1 == "RAM" && p.Componente2 == "Motherboard"));
    }

    [Fact]
    public void VerificarCompatibilidade_RAMeMotherboard_Incompativel_TipoRamDiferente()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR4, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.False(resultado.Compativel);
        Assert.Contains(resultado.Problemas, p => p.Componente1 == "RAM" && p.Componente2 == "Motherboard" && p.Severidade == "Critico");
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("Tipo de RAM incompatível"));
    }

    [Fact]
    public void VerificarCompatibilidade_RAMeMotherboard_Incompativel_CapacidadeExcedeMaximo()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 64, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 32, QuantidadeModulos = 4 }; // 128GB total
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.False(resultado.Compativel);
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("Capacidade RAM") && p.Problema.Contains("excede máximo"));
    }

    [Fact]
    public void VerificarCompatibilidade_RAMeMotherboard_Incompativel_ModulosExcedemSlots()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 2 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 4 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.False(resultado.Compativel);
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("Módulos RAM") && p.Problema.Contains("excede slots"));
    }

    // GPU + PSU

    [Fact]
    public void VerificarCompatibilidade_GPUePSU_Compativel_PotenciaSuficiente()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.True(resultado.Compativel);
        Assert.Empty(resultado.Problemas.Where(p => p.Componente1 == "GPU" && p.Componente2 == "PSU"));
    }

    [Fact]
    public void VerificarCompatibilidade_GPUePSU_Incompativel_PotenciaInsuficiente()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4090", TDP = 450, ConsumoRecomendado = 850, Comprimento = 340 };
        var psu = new PSU { Nome = "EVGA 600W", Potencia = 600, Certificacao = PsuCertification.Bronze };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.False(resultado.Compativel);
        Assert.Contains(resultado.Problemas, p => p.Componente1 == "GPU" && p.Componente2 == "PSU" && p.Severidade == "Critico");
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("PSU") && p.Problema.Contains("insuficiente para GPU"));
    }

    // PSU + Sistema

    [Fact]
    public void VerificarCompatibilidade_PSUeSistema_Compativel_PotenciaTotalSuficiente()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.True(resultado.Compativel);
        Assert.Empty(resultado.Problemas.Where(p => p.Componente1 == "PSU" && p.Componente2 == "Sistema"));
    }

    [Fact]
    public void VerificarCompatibilidade_PSUeSistema_Incompativel_PotenciaTotalInsuficiente()
    {
        var cpu = new CPU { Nome = "i9-13900K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 253 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 32, QuantidadeModulos = 4 };
        var gpu = new GPU { Nome = "RTX 4090", TDP = 450, ConsumoRecomendado = 850, Comprimento = 340 };
        var psu = new PSU { Nome = "EVGA 500W", Potencia = 500, Certificacao = PsuCertification.Bronze };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.False(resultado.Compativel);
        Assert.Contains(resultado.Problemas, p => p.Componente1 == "PSU" && p.Componente2 == "Sistema" && p.Severidade == "Critico");
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("PSU") && p.Problema.Contains("insuficiente para consumo total"));
    }

    // Consumo de Energia e Recomendações

    [Fact]
    public void VerificarCompatibilidade_CalculoConsumoEnergia_RetornaValorCorreto()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 16, QuantidadeModulos = 2 };
        var gpu = new GPU { Nome = "RTX 4070", TDP = 200, ConsumoRecomendado = 650, Comprimento = 300 };
        var psu = new PSU { Nome = "Corsair RM750", Potencia = 750, Certificacao = PsuCertification.Gold };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        // CPU (125) + GPU (200) + Motherboard (50) + RAM (2 * 3 = 6) + Outros (30) = 411W
        Assert.Equal(411, resultado.ConsumoTotalEnergia);
        // PSU recomendada = 411 * 1.2 = 493W (arredondado para baixo = 493)
        Assert.Equal(493, resultado.PSURecomendada);
    }

    [Fact]
    public void VerificarCompatibilidade_ComponentesNulos_NaoLancaExcecao()
    {
        var resultado = _service.VerificarCompatibilidade(null, null, null, null, null);

        Assert.NotNull(resultado);
        Assert.True(resultado.Compativel);
        Assert.Empty(resultado.Problemas);
    }

    [Fact]
    public void VerificarCompatibilidade_ParcialComponentes_ApenasValidadoresRelevantes()
    {
        var cpu = new CPU { Nome = "i5-13600K", Fabricante = "Intel", Socket = CpuSocket.LGA1700, TDP = 125 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR5, CapacidadeMaximaRam = 128, SlotsRam = 4 };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, null, null, null);

        Assert.NotNull(resultado);
        Assert.True(resultado.Compativel);
        Assert.Empty(resultado.Problemas);
    }

    // Múltiplos Problemas

    [Fact]
    public void VerificarCompatibilidade_MultiplosProblemas_RetornaTodosProblemas()
    {
        var cpu = new CPU { Nome = "Ryzen 7 5800X", Fabricante = "AMD", Socket = CpuSocket.AM4, TDP = 105 };
        var motherboard = new Motherboard { Nome = "ROG MAXIMUS", Socket = CpuSocket.LGA1700, TipoRamSuportado = RamType.DDR4, CapacidadeMaximaRam = 64, SlotsRam = 2 };
        var ram = new RAM { Nome = "Kingston Fury", Tipo = RamType.DDR5, Capacidade = 32, QuantidadeModulos = 4 };
        var gpu = new GPU { Nome = "RTX 4090", TDP = 450, ConsumoRecomendado = 850, Comprimento = 340 };
        var psu = new PSU { Nome = "EVGA 500W", Potencia = 500, Certificacao = PsuCertification.Bronze };

        var resultado = _service.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);

        Assert.False(resultado.Compativel);
        Assert.True(resultado.Problemas.Count >= 4); // Socket, RAM type, GPU/PSU, PSU/Sistema
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("Socket incompatível"));
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("Tipo de RAM incompatível"));
        Assert.Contains(resultado.Problemas, p => p.Problema.Contains("PSU") && p.Problema.Contains("insuficiente"));
    }
}
