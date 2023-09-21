using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Cuentas Bancarias de Destino
/// </summary>
public partial class ActCuentaDestino
{
    public int Id { get; }

    public string NumeroCuenta { get; }

    public string NombreBanco { get; }
}
