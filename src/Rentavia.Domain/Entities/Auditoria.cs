namespace Rentavia.Domain.Entities;

public class Auditoria
{
    public int Id { get; set; }
    public DateTimeOffset FechaUtc { get; set; } = DateTimeOffset.UtcNow;
    public string UsuarioId { get; set; } = "";
    public string Accion { get; set; } = "";
    public string Entidad { get; set; } = "";
    public int RegistroId { get; set; }
    public string? RegistroClave { get; set; }
}
