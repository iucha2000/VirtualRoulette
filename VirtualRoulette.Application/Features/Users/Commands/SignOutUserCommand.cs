﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Application.Features.Users.Commands
{
    public class SignOutUserCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}
