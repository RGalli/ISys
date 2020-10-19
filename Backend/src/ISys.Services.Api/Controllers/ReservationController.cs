using System;
using ISys.Application.Interfaces;
using ISys.Application.ViewModels;
using ISys.Domain.Core.Bus;
using ISys.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISys.Services.Api.Controllers
{
    [Authorize]
    [Route("api")]
    public class ReservationController : ApiController
    {
        private readonly IReservationAppService _ReservationAppService;

        public ReservationController(
            IReservationAppService ReservationAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _ReservationAppService = ReservationAppService;
        }

        [AllowAnonymous]
        [HttpGet("v1/Reservation")]
        public IActionResult Get()
        {
            return Response(_ReservationAppService.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("v1/Reservation/Room/{id:guid}")]
        public IActionResult GetReservations(Guid id)
        {
            return Response(_ReservationAppService.GetAllByRoom(id));
        }

        [AllowAnonymous]
        [HttpGet("v1/Reservation/{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var ReservationViewModel = _ReservationAppService.GetById(id);

            return Response(ReservationViewModel);
        }

        [AllowAnonymous]
        [HttpGet("v1/Reservation")]
        public IActionResult Post([FromBody] ReservationViewModel ReservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(ReservationViewModel);
            }

            _ReservationAppService.Register(ReservationViewModel);

            return Response(ReservationViewModel);
        }

        [Authorize(Policy = "CanWriteReservationData")]
        [HttpPost("v1/Reservation")]
        public IActionResult Post([FromBody] ReservationViewModel ReservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(ReservationViewModel);
            }

            _ReservationAppService.Register(ReservationViewModel);

            return Response(ReservationViewModel);
        }

        [Authorize(Policy = "CanWriteReservationData")]
        [HttpPut("v1/Reservation/{id:guid}")]
        public IActionResult Put([FromBody] ReservationViewModel ReservationViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(ReservationViewModel);
            }

            _ReservationAppService.Update(ReservationViewModel);

            return Response(ReservationViewModel);
        }

        //[Authorize(Policy = "CanRemoveReservationData")]
        [HttpDelete("v1/Reservation/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            _ReservationAppService.Remove(id);

            return Response();
        }

        [AllowAnonymous]
        [HttpGet("v1/Reservation/history/{id:guid}")]
        public IActionResult History(Guid id)
        {
            var ReservationHistoryData = _ReservationAppService.GetAllHistory(id);
            return Response(ReservationHistoryData);
        }
    }
}
