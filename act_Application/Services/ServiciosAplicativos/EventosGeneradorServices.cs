using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace act_Application.Services.ServiciosAplicativos
{
    public class EventosGeneradorServices
    {
        private readonly ActDesarrolloContext _context;
        public EventosGeneradorServices(ActDesarrolloContext context)
        {
            _context = context;
        }
        public async Task CrearEvento(int Id, int IdUser, DateTime FechaInicio, DateTime FechaFinalizacion, [Bind("Id,IdEven,IdPrestamo,IdUser,ParticipantesId,NombrePId,FechaGeneracion,FechaInicio,FechaFinalizacion,Estado")] ActEvento actEvento)
        {
            try
            {
                actEvento.IdPrestamo = Id;
                actEvento.IdUser = IdUser;
                actEvento.ParticipantesId = "";
                actEvento.NombresPId = "";
                actEvento.FechaGeneracion = DateTime.Now;
                actEvento.FechaInicio = FechaInicio;
                actEvento.FechaFinalizacion = FechaFinalizacion;
                actEvento.Estado = "EN ESPERA";
                /*ESTADOS:
                            EN ESPERA (A LA CONFIRMACION DE ACEPTAR CONDICIONES).
                            CONCURSO (A LA ESPERA DE KUE NUEVOS SOCIOS SE UNAN).
                            RECHAZADO (NO ACEPTA LAS CONDICIONES).
                            FINALIZADO (EL CONCURSO TERMINO Y SE SELECCIONARON LOS PARTICIPANTES.*/
                _context.Add(actEvento);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EventosGeneradorServices - CrearEvento | Problemas al momento de crear el Registro de la Evento");
                Console.WriteLine($"Detalles del error: {ex.Message}");
            }
        }
        public async Task ActualizarEvento(int Id, string Estado, [Bind("Id,IdEven,IdPrestamo,IdUser,ParticipantesId,NombrePId,FechaGeneracion,FechaInicio,FechaFinalizacion,Estado")] ActEvento actEvento)
        {
            try
            {
                var eobj = (ActEvento)new EventosRepository().OperacionesEventos(6, Id, 0, "");
                if(eobj != null)
                {
                    actEvento.Id = eobj.Id;
                    actEvento.IdEven = eobj.IdEven;
                    actEvento.IdPrestamo = Id;
                    actEvento.ParticipantesId = eobj.ParticipantesId;
                    actEvento.NombreUsuario = eobj.NombreUsuario;
                    actEvento.FechaGeneracion = eobj.FechaGeneracion;
                    actEvento.FechaInicio = eobj.FechaInicio;
                    actEvento.FechaFinalizacion = eobj.FechaFinalizacion;
                    if(Estado == "APROBADO")
                    {
                        actEvento.Estado = "CONCURSO";
                    }
                    else if(Estado != "APROBADO")
                    {
                        actEvento.Estado = "CANCELADO";
                    }
                    _context.Update(actEvento);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("Error en al momento de obtener los datos originales de Evento por medio del IdPrestamo");
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"ActualizarEvento - EventosGeneradorServices | Hubo un problema al actualizar el registro del Estado de Evento con el Id de Prestamo.{Id}");
                Console.WriteLine($"Detalles del error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ActualizarEvento - EventosGeneradorServices | Problemas al momento de actualizar el Registro de la Evento");
                Console.WriteLine($"Detalles del error: {ex.Message}");
            }
        }
    }
}
