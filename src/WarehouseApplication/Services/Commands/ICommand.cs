using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Services.Commands
{
    public interface ICommand<out Response> : IRequest<Response>
    {
    }
}
